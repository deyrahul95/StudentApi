using Microsoft.EntityFrameworkCore;
using StudentApi.DB.Entities;

namespace StudentApi.DB;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Student> Students{ get; set; }
}
