using Serilog.Events;
using Serilog;
using System.Diagnostics;
using HelloRazor.Lib;
using CommonLib;
using System.Reflection;
using MovieLibrary.Data;
using HelloRazor.Middleware;

public class Program
{
    static List<IModule> modules = new();

    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("HelloRazor", LogEventLevel.Debug)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateBootstrapLogger(); // <-- Change this line!

        var builder = WebApplication.CreateBuilder(args);

        try
        {
            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console());

            builder.Services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(sp => sp.GetService<ILogger<Program>>()!);

            modules = LoadModules(builder.Configuration);
            RegisterExternalServices(builder.Services, builder.Configuration);
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Builder failed");
        }

        var app = builder.Build();

        await InititalizeExternalServices(app.Services);

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
                //app.UseCustomRequestLogging();
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
    }

    private static List<IModule> LoadModules(IConfiguration configuration)
    {
        try
        {
            var modules = new List<IModule>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Console.WriteLine($"Searching IModule impl in Assembly {assembly}");
                var types = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IModule)) && t.IsClass);
                foreach (var type in types)
                {
                    if (Activator.CreateInstance(type) is IModule module)
                    {
                        modules.Add(module);
                    }
                }
            }
            var candidates = configuration.GetValue<string>("Modules", "")!
                .Split(",", StringSplitOptions.TrimEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, s));

            foreach (var file in candidates.Where(c=>!assemblies.Any(a=>a.Location == c)))
            {
                List<IModule> fromFile = Assembly.LoadFrom(file).GetTypes().Where(t => t.IsAssignableTo(typeof(IModule)) && t.IsClass)
                .Select(t => Activator.CreateInstance(t) as IModule)
                .Where(m => m != null)
                .Select(m => m!)
                .ToList() ?? new List<IModule>();
                modules.AddRange(fromFile);
            }

            return modules.Distinct().ToList();
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error loading modules");
            return new List<IModule>();
        }
    }

    private static void RegisterExternalServices(IServiceCollection services, IConfiguration configuration)
    {
        foreach (var module in modules)
        {
            module.ConfigureServices(services, configuration);
        }
    }

    private static async Task InititalizeExternalServices(IServiceProvider provider)
    {
        try
        {
            await Task.WhenAll(modules.Select(m => m.InitializeServices(provider)));
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "External initialization failed");
            throw;
        }
    }
}