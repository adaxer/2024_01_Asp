using MovieLibrary.Models;

namespace MovieLibrary.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetMovies();

    Task<Movie?> GetMovieById(int id);

    Task<Movie?> SaveMovie(Movie movie);

    Task RemoveMovieById(int value);

    Task<Dictionary<string, int>> GetMovieCountForGenres();
}
