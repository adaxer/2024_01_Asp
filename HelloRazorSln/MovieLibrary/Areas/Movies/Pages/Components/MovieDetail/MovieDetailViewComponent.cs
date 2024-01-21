using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Interfaces;

namespace MovieLibrary.Areas.Movies.Pages.Components.MovieDetail;

public class MovieDetailViewComponent : ViewComponent
{
    private readonly IMovieService service;

    public MovieDetailViewComponent(IMovieService service)
    {
        this.service = service;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var data = await GetDataAsync();
        return View("MovieDetailView", data);
    }

    private Task<Dictionary<string, int>> GetDataAsync() => service.GetMovieCountForGenres();
}