namespace StudentApi.Models;

public record StudentDto(
    Guid Id,
    string FirstName,
    string LastName,
    int Roll,
    int Age,
    string PhoneNumber,
    string EmailAddress,
    string Gender,
    string Education,
    string Occupation,
    int Experience,
    decimal Salary,
    string MaritalStatus,
    int NumberOfChildren,
    DateTime LastUpdated
);