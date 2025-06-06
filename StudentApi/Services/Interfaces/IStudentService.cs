using StudentApi.Common;
using StudentApi.Models;

namespace StudentApi.Services.Interfaces;

public interface IStudentService
{
    Task<ServiceResult<List<StudentDto>>> GetAllStudents(CancellationToken token = default);
    Task<ServiceResult<StudentDto>> GetStudent(Guid id, CancellationToken token = default);
    Task<ServiceResult> AddStudents(IFormFile file, CancellationToken token = default);
}
