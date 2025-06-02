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
        var postgreSql = configuration.GetConnectionString("DefaultConnection") ?? "";

        services.AddHealthChecks().AddNpgSql(postgreSql, name: "PostgreSQL");
        // .AddRedis(builder.Configuration.GetConnectionString("Redis"), name: "Redis");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(postgreSql));

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentService, StudentService>();

        services.AddScoped<IFileExtractor, ExcelFileExtractor>();

        return services;
    }
}
