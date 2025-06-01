namespace StudentApi.Constants;

public class ApiConstants
{
    public static readonly HashSet<string> ValidFileTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        ".xlsx", ".xls"
    };
}
