using Microsoft.EntityFrameworkCore;
using StudentApi.DB;
using StudentApi.Repositories;
using StudentApi.Services;
using StudentApi.Services.Interfaces;

namespace StudentApi.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dbConnectionString));

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentService, StudentService>();

        services.AddScoped<IFileExtractor, ExcelFileExtractor>();

        return services;
    }
}
