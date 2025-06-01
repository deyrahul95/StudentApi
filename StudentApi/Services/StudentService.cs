using System.Net;
using StudentApi.Common;
using StudentApi.Extensions;
using StudentApi.Models;
using StudentApi.Repositories;
using StudentApi.Services.Interfaces;

namespace StudentApi.Services;

public class StudentService(
    IStudentRepository studentRepository,
    ILogger<StudentService> logger) : IStudentService
{
    public async Task<ServiceResult> AddStudents(CancellationToken token = default)
    {
        try
        {
            await Task.CompletedTask;
            return new ServiceResult(
                statusCode: HttpStatusCode.Created,
                message: "Students added into database successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                message: "Failed to add students. Error: {@Error}",
                ex.Message);
            return new ServiceResult(
                statusCode: HttpStatusCode.InternalServerError,
                message: ex.Message);
        }
    }

    public async Task<ServiceResult<List<StudentDto>>> GetAllStudents(CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Start fetching students data.");
            var students = await studentRepository.GetAllAsync(token);

            var studentDtoList = students
                .OrderByDescending(x => x.LastUpdated)
                .ToDtoList();

            logger.LogInformation(
                "Students data fetched successfully. Count: {Count}",
                studentDtoList.Count);

            return new ServiceResult<List<StudentDto>>(
                statusCode: HttpStatusCode.OK,
                message: "Students fetched successfully",
                data: studentDtoList);
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                message: "Failed to fetched students data. Error: {@Error}",
                ex.Message);
            return new ServiceResult<List<StudentDto>>(
                statusCode: HttpStatusCode.InternalServerError,
                message: ex.Message);
        }
    }

    public async Task<ServiceResult<StudentDto>> GetStudent(Guid id, CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Start fetching student data. ID: {ID}", id);
            var student = await studentRepository.GetByIdAsync(id: id, token: token);

            if (student is null)
            {
                logger.LogWarning("Student not found. ID: {ID}", id);
                return new ServiceResult<StudentDto>(
                    statusCode: HttpStatusCode.NotFound,
                    message: $"Student with id: {id} not found in our database");
            }

            logger.LogInformation("Student data fetched successfully. ID: {ID}", student.Id);

            return new ServiceResult<StudentDto>(
                statusCode: HttpStatusCode.OK,
                message: "Student fetched successfully",
                data: student.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                message: "Failed to fetched student data. Error: {@Error}, ID: {ID}",
                ex.Message,
                id);
            return new ServiceResult<StudentDto>(
                statusCode: HttpStatusCode.InternalServerError,
                message: ex.Message);
        }
    }
}
