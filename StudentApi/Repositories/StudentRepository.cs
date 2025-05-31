using Microsoft.EntityFrameworkCore;
using StudentApi.DB;
using StudentApi.DB.Entities;

namespace StudentApi.Repositories;

/// <summary>
/// Student repository to retrieve and create students data
/// </summary>
/// <param name="dbContext">An instance of <see cref="AppDbContext"/></param>
/// <param name="logger">An instance of <see cref="ILogger"/></param>
public class StudentRepository(
    AppDbContext dbContext,
    ILogger<StudentRepository> logger) : IStudentRepository
{
    /// <summary>
    /// Add student data
    /// </summary>
    /// <param name="student"><see cref="Student"/> data to be added</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Returns void</returns>
    public async Task AddAsync(Student student, CancellationToken token = default)
    {
        var result = await dbContext.Students.AddAsync(student, token);

        await dbContext.SaveChangesAsync(token);
        logger.LogInformation("Student Added. Student: {@Student}", result.Entity);
    }

    /// <summary>
    /// Retrieved all students data
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>Returns an IEnumerable list of <see cref="Student"/> data</returns>
    public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken token = default)
    {
        return await dbContext.Students.AsNoTracking()
            .ToListAsync(token);
    }

    /// <summary>
    /// Retrieve student data by it's ID
    /// </summary>
    /// <param name="id">Student unique identifier number</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Returns <see cref="Student"/> data if found else Null</returns>
    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await dbContext.Students.AsNoTracking()
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(token);
    }
}
