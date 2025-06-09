using StudentApi.Common;
using StudentApi.Models;

namespace StudentApi.Services.Interfaces;

public interface IStudentService
{
    Task<ServiceResult<PagedResult<StudentDto>>> GetAllStudents(
        StudentQueryParameters parameters,
        CancellationToken token = default);
    Task<ServiceResult<StudentDto>> GetStudent(Guid id, CancellationToken token = default);
    Task<ServiceResult> ImportStudents(IFormFile file, CancellationToken token = default);
}
