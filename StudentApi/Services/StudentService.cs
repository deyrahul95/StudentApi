using System.Net;
using Microsoft.EntityFrameworkCore;
using StudentApi.Common;
using StudentApi.Extensions;
using StudentApi.Models;
using StudentApi.Repositories;
using StudentApi.Services.Interfaces;

namespace StudentApi.Services;

public class StudentService(
    IStudentRepository studentRepository,
    IFileExtractor fileExtractor,
    ILogger<StudentService> logger) : IStudentService
{
    public async Task<ServiceResult> ImportStudents(IFormFile file, CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Start extracting student data from excel file. FileName: {FileName}", file.FileName);
            var excelStudentModels = await fileExtractor.ExtractFileData<ExcelStudentModel>(
               file,
               token,
               (row, error) => logger.LogError("Row {@Row} error: {@Error}", row, error));

            logger.LogInformation("Student data extracted successfully. Count: {Count}", excelStudentModels.Count);

            await studentRepository.ImportAsync(excelStudentModels.ToEntityList(), token);
            logger.LogInformation("Data inserted into DB");

            return new ServiceResult(
                statusCode: HttpStatusCode.Created,
                message: "Students imported successfully");
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

    public async Task<ServiceResult<PagedResult<StudentDto>>> GetAllStudents(
        PaginationParameters parameters,
        CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Start fetching students data.");
            var query = studentRepository.GetQueryable();

            var totalCount = await query.CountAsync(token);

            var students = await query
                .OrderByDescending(s => s.LastUpdated)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken: token);

            logger.LogInformation(
                "Students data fetched successfully. Count: {Count}",
                students.Count);

            var pagedResult = new PagedResult<StudentDto>(
                items: students.ToDtoList(),
                count: totalCount,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize);

            return new ServiceResult<PagedResult<StudentDto>>(
                statusCode: HttpStatusCode.OK,
                message: "Students fetched successfully",
                data: pagedResult);
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                message: "Failed to fetched students data. Error: {@Error}",
                ex.Message);
            return new ServiceResult<PagedResult<StudentDto>>(
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
