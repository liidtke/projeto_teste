using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.Domain;
using WebApi.Domain.Commands;
using WebApi.Domain.Queries;
using WebApi.Domain.SharedKernel;
using WebApi.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<WebApiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IIncrementalIdCacheService, IncrementalIdCacheService>();
builder.Services.AddScoped<CreateArrivalCommand>();
builder.Services.AddScoped<CreateTriageCommand>();
builder.Services.AddScoped<CreatePatientCommand>();
builder.Services.AddScoped<UpdateArrivalCommand>();
builder.Services.AddScoped<UpdatePatientCommand>();
builder.Services.AddScoped<PatientArrivalsQuery>();
builder.Services.AddScoped<PatientQuery>();
builder.Services.AddScoped<TriageQuery>();
builder.Services.AddScoped<GetNextNumberQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();
app.MapControllers();

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetService<WebApiContext>();
        db?.Database.Migrate();
    }
    catch (Exception e)
    {
        var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
        logger.LogError(e, "Error applying migrations");
    }
}
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}