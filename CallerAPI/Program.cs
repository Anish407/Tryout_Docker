using CallerApi.Infra.Handler;
using CallerApi.Infra.Repository;
using CalllerApi.Core.Repository;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IStudentHandler,StudentHandler>();
// Built-in OpenAPI support for .NET 10
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Map OpenAPI JSON endpoint and the built-in interactive UI
    app.MapOpenApi();
        //app.MapOpenApiUI(options =>
        //{
        //    options.DocumentPath = "/docs/v1.json";
        //    options.Path = "/docs"; // UI at /docs
        //}); // <-- exposes the UI (e.g. /openapi or configurable route)
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapControllers();

app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    return Enumerable.Range(1, 5).Select(index => new
    {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = summaries[Random.Shared.Next(summaries.Length)]
    })
    .ToArray();
});

app.Run();
