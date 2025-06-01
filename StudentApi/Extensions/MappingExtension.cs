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

    public static Student ToEntity(this ExcelStudentModel studentModel)
    {
        var student = new Student()
        {
            Id = Guid.NewGuid(),
            FirstName = studentModel.FirstName,
            LastName = studentModel.LastName,
            Roll = studentModel.Roll,
            Age = studentModel.Age,
            PhoneNumber = studentModel.PhoneNumber,
            EmailAddress = studentModel.EmailAddress,
            Gender = studentModel.Gender,
            Education = studentModel.Education,
            Occupation = studentModel.Occupation,
            Experience = studentModel.Experience,
            Salary = studentModel.Salary,
            MaritalStatus = studentModel.MaritalStatus,
            NumberOfChildren = studentModel.NumberOfChildren,
            CreatedAt = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        return student;
    }

    public static List<Student> ToEntityList(this List<ExcelStudentModel> studentModels)
    {
        if (studentModels.Count == 0)
        {
            return [];
        }

        return [.. studentModels.Select(ToEntity)];
    }
}
