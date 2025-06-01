using StudentApi.Common;
using StudentApi.Models;

namespace StudentApi.Services;

public interface IStudentService
{
    Task<ServiceResult<List<StudentDto>>> GetAllStudents(CancellationToken token = default);
    Task<ServiceResult<StudentDto>> GetStudent(Guid id, CancellationToken token = default);
    Task<ServiceResult> AddStudents(CancellationToken token = default);
}
