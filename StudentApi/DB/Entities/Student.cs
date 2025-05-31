using System.ComponentModel.DataAnnotations;
using StudentApi.Constants;
using StudentApi.Enums;

namespace StudentApi.DB.Entities;

/// <summary>
/// Student entity
/// </summary>
public class Student
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(DbConstants.MaxFirstNameLength)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(DbConstants.MaxLastNameLength)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public int Roll { get; set; }

    [Required]
    public int Age { get; set; }

    [MaxLength(DbConstants.MaxPhoneLength)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(DbConstants.MaxEmailLength)]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    [MaxLength(DbConstants.MaxEducationLength)]
    public string Education { get; set; } = string.Empty;

    [MaxLength(DbConstants.MaxOccupationLength)]
    public string Occupation { get; set; } = string.Empty;

    public int Experience { get; set; }

    public decimal Salary { get; set; }

    public MaritalStatus MaritalStatus { get; set; }

    public int NumberOfChildren { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
