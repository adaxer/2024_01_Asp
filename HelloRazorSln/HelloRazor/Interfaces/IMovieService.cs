using HelloRazor.Models;

namespace HelloRazor.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetMovies();

    Task<Movie?> GetMovieById(int id);

    Task<Movie?> SaveMovie(Movie movie);
    
    Task RemoveMovieById(int value);
    Task<Dictionary<string, int>> GetMovieCountForGenres();
}
