using StudentApi.DB.Entities;
using StudentApi.Models;

namespace StudentApi.Extensions;

public static class MappingExtension
{
    public static StudentDto ToDto(this Student student)
    {
        return new StudentDto(
            Id: student.Id,
            FirstName: student.FirstName,
            LastName: student.LastName,
            Roll: student.Roll,
            Age: student.Age,
            PhoneNumber: student.PhoneNumber,
            EmailAddress: student.EmailAddress,
            Gender: student.Gender.ToString(),
            Education: student.Education,
            Occupation: student.Occupation,
            Experience: student.Experience,
            Salary: student.Salary,
            MaritalStatus: student.MaritalStatus.ToString(),
            NumberOfChildren: student.NumberOfChildren,
            LastUpdated: student.LastUpdated
        );
    }

    public static List<StudentDto> ToDtoList(this IEnumerable<Student> students)
    {
        if (students.Any() is false)
        {
            return [];
        }

        return [.. students.Select(ToDto)];
    }
}
