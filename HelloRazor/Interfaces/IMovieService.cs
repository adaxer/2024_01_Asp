using HelloRazor.Models;

namespace HelloRazor.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetMovies();
}
