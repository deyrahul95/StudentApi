using System.Threading.RateLimiting;
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

    public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // Apply rate limit per IP address
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? ApiConstants.UnknownIpAddress;

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ipAddress,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = ApiConstants.PermitLimitPerWindow,
                        Window = TimeSpan.FromSeconds(ApiConstants.WindowTimeSpanInSec),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = ApiConstants.WindowQueueLimit
                    });
            });

            // Custom response when rate limit is hit
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers.RetryAfter = $"{ApiConstants.WindowTimeSpanInSec}s";
                await context.HttpContext.Response.WriteAsync(
                    text: "Too many requests. Try again later.",
                    cancellationToken: token);
            };

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }
}
