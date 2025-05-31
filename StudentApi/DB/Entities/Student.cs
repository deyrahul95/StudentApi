using StudentApi.Enums;

namespace StudentApi.DB.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Roll { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string Education { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public int Experience { get; set; }
    public decimal Salary { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public int NumberOfChildren { get; set; }
}
