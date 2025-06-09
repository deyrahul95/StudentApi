using StudentApi.Common;
using StudentApi.Enums;

namespace StudentApi.Models;

public class StudentQueryParameters : PaginationParameters
{
    public string? SearchTerm { get; set; }

    public StudentSortField SortBy { get; set; } = StudentSortField.Name;

    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}

