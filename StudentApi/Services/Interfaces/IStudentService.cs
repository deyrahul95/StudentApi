using StudentApi.Common;
using StudentApi.Models;

namespace StudentApi.Services.Interfaces;

/// <summary>
/// Defines the contract for student-related operations such as retrieval, pagination, import, and data manipulation
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Retrieves a paged list of students based on the specified query parameters
    /// </summary>
    /// <param name="parameters">Query parameters for filtering, sorting, and pagination</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation, containing a service result with a paged list of <see cref="StudentDto"/></returns> 
    Task<ServiceResult<PagedResult<StudentDto>>> GetAllStudents(
        StudentQueryParameters parameters,
        CancellationToken token = default);

    /// <summary>
    /// Retrieves a single student by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the student</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation, containing a service result with the <see cref="StudentDto"/></returns>
    Task<ServiceResult<StudentDto>> GetStudent(Guid id, CancellationToken token = default);

    /// <summary>
    /// Imports student data from the provided file
    /// </summary>
    /// <param name="file">The file containing student data to import</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation, containing a service result indicating the outcome of the import</returns>
    Task<ServiceResult> ImportStudents(IFormFile file, CancellationToken token = default);
}
