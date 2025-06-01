using System.Reflection;
using ClosedXML.Excel;
using StudentApi.Models;
using StudentApi.Services.Interfaces;

namespace StudentApi.Services;

public class ExcelFileExtractor : IFileExtractor
{
    public async Task<List<T>> ExtractFileData<T>(
        IFormFile file,
        CancellationToken token,
        Action<int, string>? onRowError = null) where T : class, new()
    {
        var result = new List<T>();
        var rows = await LoadWorkbookAsync(file, token);

        if (rows == null || rows.Count < 2)
        {
            return result;
        }

        var headers = rows[0];
        var propertyMap = GetHeaderMap<T>(headers);

        for (int rowIndex = 1; rowIndex < rows.Count; rowIndex++)
        {
            var row = rows[rowIndex];

            if (ConvertRowToModel(
                row,
                headers,
                propertyMap,
                out T model,
                out var errorMessages))
            {
                result.Add(model);
            }
            else
            {
                foreach (var error in errorMessages)
                {
                    onRowError?.Invoke(rowIndex + 1, error);
                }
            }
        }

        return result;
    }

    private static async Task<List<List<string>>?> LoadWorkbookAsync(IFormFile file, CancellationToken token)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, token);
        stream.Position = 0;

        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.FirstOrDefault();

        if (worksheet == null)
        {
            return null;
        }

        var range = worksheet.RangeUsed();

        if (range == null)
        {
            return null;
        }

        return [.. range.RowsUsed()
            .Select(row => row.Cells()
                .Select(c => c.GetString().Trim())
                .ToList())];
    }

    private static Dictionary<string, PropertyInfo> GetHeaderMap<T>(List<string> headers)
    {
        var map = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite);

        foreach (var prop in properties)
        {
            var attr = prop.GetCustomAttribute<ColumnAttribute>();
            var header = attr?.HeaderName ?? prop.Name;

            // Only map headers that are actually present in the Excel file
            if (headers.Contains(header, StringComparer.OrdinalIgnoreCase))
                map[header] = prop;
        }

        return map;
    }

    private static bool ConvertRowToModel<T>(
    List<string> row,
    List<string> headers,
    Dictionary<string, PropertyInfo> propertyMap,
    out T model,
    out List<string> errorMessages) where T : new()
    {
        model = new T();
        errorMessages = [];

        for (int i = 0; i < headers.Count && i < row.Count; i++)
        {
            var header = headers[i];

            if (propertyMap.TryGetValue(header, out var prop) is false)
            {
                continue;
            }

            var rawValue = row[i];

            try
            {
                var value = ConvertValue(rawValue, prop.PropertyType);
                prop.SetValue(model, value);
            }
            catch (Exception ex)
            {
                errorMessages.Add($"Column '{header}': {ex.Message}");
            }
        }

        return errorMessages.Count == 0;
    }

    private static object? ConvertValue(string input, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var type = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (type == typeof(string))
        {
            return input;
        }

        if (type.IsEnum)
        {
            if (Enum.TryParse(
                enumType: type,
                value: input.Trim(),
                ignoreCase: true,
                result: out var enumValue))
            {
                return enumValue;
            }
            else
            {
                throw new InvalidCastException($"Unable to parse '{input}' to enum '{type.Name}'.");
            }
        }

        if (type == typeof(long))
        {
            if (long.TryParse(input, out var l)) return l;

            if (double.TryParse(input, out var dd))
            {
                if (dd >= long.MinValue && dd <= long.MaxValue)
                    return (long)dd;
            }

            throw new InvalidCastException($"'{input}' is not a valid long integer.");
        }

        if (type == typeof(int) && int.TryParse(input, out var i)) return i;
        if (type == typeof(double) && double.TryParse(input, out var d)) return d;
        if (type == typeof(decimal) && decimal.TryParse(input, out var dec)) return dec;
        if (type == typeof(bool) && bool.TryParse(input, out var b)) return b;
        if (type == typeof(DateTime) && DateTime.TryParse(input, out var dt)) return dt;

        // Fallback to general conversion
        return Convert.ChangeType(input, type);
    }
}
