namespace StudentApi.Models;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnAttribute(string headerName) : Attribute
{
    public string HeaderName { get; } = headerName;
}
