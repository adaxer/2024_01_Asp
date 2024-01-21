using CommonLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieLibrary.Data;
using MovieLibrary.Interfaces;
using MovieLibrary.Services;
using System.Diagnostics;

namespace MovieLibrary;
public class MovieModule : IModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // MovieService
        services.AddScoped<IMovieService, MovieService>();

        // Db-Context
        services.AddDbContext<MoviesContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("MoviesContext") ?? throw new InvalidOperationException("Connection string 'MoviesContext' not found."));
            // options.EnableSensitiveDataLogging().LogTo(s => Console.WriteLine($"FromLogTo: {s}"));
        });
    }

    public Task InitializeServices(IServiceProvider services)
    {
        var environment = services.GetService<IHostEnvironment>()!;
        if (environment.IsDevelopment())
        {
            using var scope = services.CreateScope();
            try
            {
                var db = scope.ServiceProvider.GetService<MoviesContext>();
                SeedData.CreateDummyData(db!);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Seeding failed. Make sure to have a directory App_Data created in the project, and performed dotnet ef database update!\r\n{ex}");
                throw;
            }
        }
        return Task.CompletedTask;
    }
}
