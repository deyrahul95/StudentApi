namespace StudentApi.Common;

/// <summary>
/// Provides properties to control pagination of query results
/// Includes page number and page size with a maximum limit on page size
/// </summary>
public class PaginationParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
