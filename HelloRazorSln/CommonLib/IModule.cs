using Microsoft.Extensions.DependencyInjection;

namespace CommonLib;

public interface IModule
{
    void ConfigureServices(IServiceCollection services);
}
