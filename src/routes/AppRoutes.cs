using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;  
using MiPortal.Data; 
using MiPortal;       
public static class AppRoutes
{
    public static void Configure(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/usuarios", async (ApplicationDbContext context) =>
        {
            var users = await context.Users.ToListAsync(); 
            return Results.Ok(users);
        });

        endpoints.MapGet("/weatherforecast", () =>
        {
            Console.WriteLine("Accediendo al pronóstico del tiempo...");

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

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
        .WithName("GetWeatherForecast")
        .WithOpenApi();
    }
}
