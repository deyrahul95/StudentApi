using StudentApi.DB.Entities;

namespace StudentApi.Repositories;

/// <summary>
/// Student repository to retrieve and create students data
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Retrieves an <see cref="IQueryable{Student}"/> representing the collection of students
    /// </summary>
    /// <returns>An <see cref="IQueryable{Student}"/> that can be used to query student data</returns>
    IQueryable<Student> GetQueryable();

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
