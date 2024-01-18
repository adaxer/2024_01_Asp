using CommonLib;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace MovieLibrary;
public class MovieModule : IModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Db-COntext;
        // MovieService
        // Seiten verankern - ?
        Trace.WriteLine(nameof(MovieModule));
    }
}
