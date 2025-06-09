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
    public async Task ImportAsync(List<Student> student, CancellationToken token = default)
    {
        await dbContext.Students.AddRangeAsync(student, token);
        await dbContext.SaveChangesAsync(token);
        logger.LogInformation("All Student Added.");

    }

    /// <summary>
    /// Add student data
    /// </summary>
    /// <param name="student"><see cref="Student"/> data to be added</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Returns void</returns>
    public async Task AddAsync(Student student, CancellationToken token = default)
    {
        var result = await dbContext.Students.AddAsync(
            entity: student,
            cancellationToken: token);

        await dbContext.SaveChangesAsync(token);
        logger.LogInformation("Student Added. Student: {@Student}", result.Entity);
    }

    /// <summary>
    /// Retrieves an <see cref="IQueryable{Student}"/> representing the collection of students
    /// </summary>
    /// <returns>An <see cref="IQueryable{Student}"/> that can be used to query student data</returns>
    public IQueryable<Student> GetQueryable()
    {
        return dbContext.Students.AsNoTracking();
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
            .FirstOrDefaultAsync(token);
    }
}
