using StudentApi.Common;
using StudentApi.Enums;

namespace StudentApi.Models;

/// <summary>
/// Represents query parameters for filtering, sorting, and paginating students
/// Inherits pagination properties from <see cref="PaginationParameters"/>
/// </summary>
public class StudentQueryParameters : PaginationParameters
{
    public string? SearchTerm { get; set; }

    public StudentSortField SortBy { get; set; } = StudentSortField.Name;

    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}