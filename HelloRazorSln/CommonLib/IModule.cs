using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLib;

public interface IModule
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);

    Task InitializeServices(IServiceProvider services);
}
