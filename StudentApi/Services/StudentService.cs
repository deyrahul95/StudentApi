using System.Net;
using Microsoft.EntityFrameworkCore;
using StudentApi.Common;
using StudentApi.DB.Entities;
using StudentApi.Enums;
using StudentApi.Extensions;
using StudentApi.Models;
using StudentApi.Repositories;
using StudentApi.Services.Interfaces;

namespace StudentApi.Services;

/// <summary>
/// Provides student-related operations including data retrieval, pagination, sorting, and import
/// Implements <see cref="IStudentService"/>
/// </summary>
public class StudentService(
    IStudentRepository studentRepository,
    IFileExtractor fileExtractor,
    ICacheService cacheService,
    ILogger<StudentService> logger) : IStudentService
{
    /// <summary>
    /// Imports student data from the provided file
    /// </summary>
    /// <param name="file">The file containing student data to import</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>A task representing the asynchronous operation, containing a service result indicating the outcome of the import</returns>
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

    /// <summary>
    /// Retrieves a paged list of students based on query parameters
    /// </summary>
    /// <param name="parameters">Parameters for filtering, sorting, and pagination</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>A task representing the asynchronous operation, containing a service result with a paged list of <see cref="StudentDto"/></returns>
    public async Task<ServiceResult<PagedResult<StudentDto>>> GetAllStudents(
        StudentQueryParameters parameters,
        CancellationToken token = default)
    {
        try
        {
            logger.LogInformation("Start fetching students data.");

            var cacheKey = GenerateCachedKey(parameters);
            var cached = await cacheService.GetAsync<PagedResult<StudentDto>>(cacheKey);

            if (cached is not null)
            {
                logger.LogInformation(
                    "[Cached] Retrieved students data from cache successfully. Count: {Count}, Key: {Key}",
                    cached.Items.Count,
                    cacheKey);

                return new ServiceResult<PagedResult<StudentDto>>(
                    statusCode: HttpStatusCode.OK,
                    message: "Students fetched successfully",
                    data: cached);
            }

            var query = studentRepository.GetQueryable();
            query = ApplySearching(parameters: parameters, query: query);
            query = ApplySorting(query: query, sortBy: parameters.SortBy, direction: parameters.SortDirection);

            var pagedResult = await ApplyPagination(
                query: query,
                parameters: parameters,
                token: token);
            logger.LogInformation(
                "Students data fetched successfully. Count: {Count}",
                pagedResult.Items.Count);

            await cacheService.SetAsync(key: cacheKey, item: pagedResult);
            logger.LogInformation(
                "[Cached] Students data cached successfully. Count: {Count}, Key: {Key}",
                pagedResult.Items.Count,
                cacheKey);

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

    /// <summary>
    /// Retrieves a student by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the student</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>A task representing the asynchronous operation, containing a service result with the <see cref="StudentDto"/></returns>
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

    /// <summary>
    /// Generates a unique cache key based on the provided <see cref="StudentQueryParameters"/>.
    /// </summary>
    /// <param name="parameters">The query parameters used to filter and paginate student data.</param>
    /// <returns>A string representing the generated cache key.</returns>
    private static string GenerateCachedKey(StudentQueryParameters parameters)
    {
        return $"students_{parameters.PageNumber}_{parameters.PageSize}_{parameters.SearchTerm}_{parameters.SortBy}_{parameters.SortDirection}";
    }

    /// <summary>
    /// Applies search filtering to the student query based on the provided query parameters
    /// </summary>
    /// <param name="parameters">The query parameters containing search criteria</param>
    /// <param name="query">The initial student query to apply filtering to</param>
    /// <returns>The filtered <see cref="IQueryable{Student}"/> based on search criteria</returns>
    private static IQueryable<Student> ApplySearching(
        StudentQueryParameters parameters,
        IQueryable<Student> query)
    {
        if (string.IsNullOrEmpty(parameters.SearchTerm) == false)
        {
            var term = parameters.SearchTerm.ToLower();

            query = query.Where(s =>
                s.FirstName.ToLower().Contains(term) ||
                s.LastName.ToLower().Contains(term) ||
                s.Roll.ToString().Contains(term) ||
                s.Age.ToString().Contains(term)
            );
        }

        return query;
    }

    /// <summary>
    /// Applies sorting to the given <see cref="IQueryable{Student}"/> based on the specified sort field and direction
    /// </summary>
    /// <param name="query">The student query to apply sorting to</param>
    /// <param name="sortBy">The field by which to sort the students</param>
    /// <param name="direction">The direction of the sort (ascending or descending)</param>
    /// <returns>An <see cref="IQueryable{Student}"/> with the applied sorting</returns>
    private static IQueryable<Student> ApplySorting(
        IQueryable<Student> query,
        StudentSortField sortBy,
        SortDirection direction)
    {
        var isDescending = direction == SortDirection.Desc;

        return sortBy switch
        {
            StudentSortField.Name => isDescending
                ? query
                    .OrderByDescending(s => s.FirstName)
                    .ThenByDescending(s => s.LastName)
                : query
                    .OrderBy(s => s.FirstName)
                    .ThenBy(s => s.LastName),

            StudentSortField.Roll => isDescending
                ? query.OrderByDescending(s => s.Roll)
                : query.OrderBy(s => s.Roll),

            StudentSortField.Age => isDescending
                ? query.OrderByDescending(s => s.Age)
                : query.OrderBy(s => s.Age),

            _ => query
                    .OrderBy(s => s.FirstName)
                    .ThenBy(s => s.LastName)
        };
    }

    /// <summary>
    /// Applies pagination to the given <see cref="IQueryable{Student}"/> based on the specified parameters
    /// </summary>
    /// <param name="query">The student query to paginate</param>
    /// <param name="parameters">Pagination parameters including page number and page size</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation, containing a paged result of <see cref="StudentDto"/></returns>
    private static async Task<PagedResult<StudentDto>> ApplyPagination(
        IQueryable<Student> query,
        PaginationParameters parameters,
        CancellationToken token)
    {
        var totalCount = await query.CountAsync(token);

        var students = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(cancellationToken: token);

        return new PagedResult<StudentDto>(
            items: students.ToDtoList(),
            totalCount: totalCount,
            pageNumber: parameters.PageNumber,
            pageSize: parameters.PageSize);
    }
}
