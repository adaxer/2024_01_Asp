using HelloRazor.Api;
using HelloRazor.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelloRazor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        var api = new WeatherForecastApi("https://localhost:7164");
        var greeting = await api.WeatherForecastGreetingGetAsync();
    }
}
