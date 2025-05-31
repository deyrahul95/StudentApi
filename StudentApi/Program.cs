using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();