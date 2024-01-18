using HelloRazor.Data;
using HelloRazor.Interfaces;
using HelloRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloRazor.Services;

public class MovieService : IMovieService
{
    private readonly MoviesContext db;
    private readonly ILogger<MovieService> logger;

    public MovieService(MoviesContext db, ILogger<MovieService> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public async Task<Movie?> GetMovieById(int id)
    {
        return await db.Movies.FindAsync(id);
    }

    public async Task<Dictionary<string, int>> GetMovieCountForGenres()
    {
        var result = new Dictionary<string, int>();
        var genres = await db.Movies.Select(m => m.Genre).Distinct().ToListAsync();
        foreach (var genre in genres.Where(g => g != null))
        {
            result[genre!] = await db.Movies.Where(m => m.Genre == genre).CountAsync();
        }
        return result;
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
        var movies = await db.Movies.ToListAsync();
        return movies;
    }

    public async Task RemoveMovieById(int id)
    {
        try
        {
            if (db.Movies.Find(id) is { } movie)
            {
                db.Movies.Remove(movie);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Could not delete movie {id}");
        }
    }

    public async Task<Movie?> SaveMovie(Movie movie)
    {
        try
        {
            if (db.Movies.Any(m => m.Id == movie.Id))
            {
                db.Attach(movie).State = EntityState.Modified;
            }
            else
            {
                db.Movies.Add(movie);
            }
            await db.SaveChangesAsync();
            return movie;
        }
        catch (Exception ex)
        {
            var err = $"Failed to save {movie}";
            logger.LogError(ex, err);
            throw new Exception(err, ex);
        }
    }
}
