using Microsoft.EntityFrameworkCore;
using StudentApi.DB.Entities;

namespace StudentApi.DB;

/// <summary>
/// Application database context
/// </summary>
/// <param name="options">Database context options</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
}
