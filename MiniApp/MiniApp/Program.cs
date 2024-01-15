using Serilog.Events;
using Serilog;

namespace MiniApp;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger(); // <-- Change this line!

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, services, configuration) => configuration
             .ReadFrom.Configuration(context.Configuration)
             .ReadFrom.Services(services)
             .Enrich.FromLogContext()
             .WriteTo.Console());

        builder.Services.AddScoped<DummyService>();

        var app = builder.Build();

        app.UseSerilogRequestLogging();

        app.MapGet("/", (DummyService service) => service.SayHello());

        app.MapGet("/html", async (HttpContext context) =>
        {
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<h4>Title</h4><p>Content</p>");
        });

        app.UseStaticFiles();

        app.Run();
    }
}
