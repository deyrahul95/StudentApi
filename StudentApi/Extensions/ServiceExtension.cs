using Microsoft.EntityFrameworkCore;
using StudentApi.Constants;
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
        var redis = configuration.GetConnectionString("Redis") ?? "";

        services.AddHealthChecks().AddNpgSql(postgreSql, name: "PostgreSQL")
            .AddRedis(redis, name: "Redis");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(postgreSql));

        services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = redis;
            option.InstanceName = ApiConstants.RedisInstanceName;
        });

        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IFileExtractor, ExcelFileExtractor>();

        return services;
    }
}
