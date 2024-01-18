using HelloRazor.Interfaces;
using HelloRazor.Services;
using Serilog.Events;
using Serilog;
using System.Diagnostics;
using HelloRazor.Lib;
using Microsoft.EntityFrameworkCore;
using HelloRazor.Data;
using HelloRazor;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); // <-- Change this line!

var builder = WebApplication.CreateBuilder(args);

try
{
    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddScoped<IMovieService, MovieService>();

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(sp => sp.GetService<ILogger<Program>>()!);

    builder.Services.AddDbContext<MoviesContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("MoviesContext") ?? throw new InvalidOperationException("Connection string 'MoviesContext' not found.")));
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Builder failed");
}

var app = builder.Build();

try
{
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<MoviesContext>();
        SeedData.CreateDummyData(db!);
    }
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Seeding failed. Make sure to have a directory App_Data created in the project, and performed dotnet ef database update");
    throw;
}

try
{
    var listener = new SerilogTraceListener.SerilogTraceListener("Trace");
    Trace.Listeners.Add(listener);
    Trace.Listeners.Add(new AppTraceListener());

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        //app.UseSerilogRequestLogging();
        app.UseCustomRequestLogging();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Configure failed");
}

app.Run();
