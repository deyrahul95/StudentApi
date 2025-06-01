using StudentApi.Enums;

namespace StudentApi.Models;

public class ExcelStudentModel
{
    [Column("First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Column("Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Column("Roll")]
    public long Roll { get; set; }

    [Column("Age")]
    public int Age { get; set; }

    [Column("Phone")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Column("Email")]
    public string EmailAddress { get; set; } = string.Empty;

    [Column("Gender")]
    public Gender Gender { get; set; }

    [Column("Education")]
    public string Education { get; set; } = string.Empty;

    [Column("Occupation")]
    public string Occupation { get; set; } = string.Empty;

    [Column("Experience (Years)")]
    public int Experience { get; set; }

    [Column("Salary")]
    public decimal Salary { get; set; }

    [Column("Marital Status")]
    public MaritalStatus MaritalStatus { get; set; }

    [Column("Number of Children")]
    public int NumberOfChildren { get; set; }
}