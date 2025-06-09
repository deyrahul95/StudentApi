namespace StudentApi.Common;

/// <summary>
/// Represents a paged subset of a collection of items with pagination metadata
/// </summary>
/// <typeparam name="T">The type of items in the paged result</typeparam>
public sealed class PagedResult<T>(List<T> items, int count, int pageNumber, int pageSize)
{
    /// <summary>
    /// Gets or sets the items on the current page
    /// </summary>
    public List<T> Items { get; set; } = items;

    /// <summary>
    /// Gets or sets the total number of items across all pages
    /// </summary>
    public int TotalCount { get; set; } = count;

    /// <summary>
    /// Gets or sets the current page number (1-based)
    /// </summary>
    public int PageNumber { get; set; } = pageNumber;

    /// <summary>
    /// Gets or sets the size of each page
    /// </summary>
    public int PageSize { get; set; } = pageSize;

    /// <summary>
    /// Gets the total number of pages
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Gets a value indicating whether there is a previous page
    /// </summary>
    public bool HasPrevious => PageNumber > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page
    /// </summary>
    public bool HasNext => PageNumber < TotalPages;
}