using StudentApi.DB.Entities;

namespace StudentApi.Repositories;

/// <summary>
/// Student repository to retrieve and create students data
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Retrieved all students data
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>Returns an IEnumerable list of <see cref="Student"/> data</returns>
    Task<IEnumerable<Student>> GetAllAsync(CancellationToken token = default);

    /// <summary>
    /// Retrieve student data by it's ID
    /// </summary>
    /// <param name="id">Student unique identifier number</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Returns <see cref="Student"/> data if found else Null</returns>
    Task<Student?> GetByIdAsync(Guid id, CancellationToken token = default);

    /// <summary>
    /// Add student data
    /// </summary>
    /// <param name="student"><see cref="Student"/> data to be added</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Returns void</returns>
    Task AddAsync(Student student, CancellationToken token = default);

    /// <summary>
    /// Add student data
    /// </summary>
    /// <param name="student"><see cref="List<Student>"/>A list of student data to be added</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Returns void</returns>
    Task ImportAsync(List<Student> student, CancellationToken token = default);
}
