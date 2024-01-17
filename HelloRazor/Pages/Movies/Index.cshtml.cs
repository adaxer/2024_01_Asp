using HelloRazor.Interfaces;
using HelloRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelloRazor.Pages.Movies;

public class IndexModel : PageModel
{
    private readonly IMovieService movieService;
    private readonly ILogger<IndexModel> logger;

    public IndexModel(IMovieService movieService, ILogger<IndexModel> logger)
    {
        this.movieService = movieService;
        this.logger = logger;
    }

    public List<Movie>? Movies { get; private set; }

    public async Task OnGet()
    {
        logger.LogInformation("Get: Loading Movie data");
        this.Movies = (await movieService.GetMovies()).ToList();
    }
}
