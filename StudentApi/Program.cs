using Scalar.AspNetCore;
using Serilog;
using StudentApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddCustomRateLimiter();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        {
            options.WithTitle("Students API");
            options.WithTheme(ScalarTheme.BluePlanet);
            options.WithSidebar(true);
        });
}

app.UseSerilogRequestLogging();

app.UseRateLimiter();

app.MapControllers();

app.Run();