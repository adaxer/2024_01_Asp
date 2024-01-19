using Microsoft.AspNetCore.Mvc.RazorPages;
using HelloRazor.Models;
using HelloRazor.Interfaces;

namespace HelloRazor.Pages.Movies;

public class IndexModel : PageModel
{
    private readonly IMovieService service;

    public IndexModel(IMovieService service)
    {
        this.service = service;
    }

    public IList<Movie> Movies { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Movies = (await service.GetMovies()).ToList();
    }
}
