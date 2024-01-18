using HelloRazor.Data;
using HelloRazor.Interfaces;
using HelloRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloRazor.Services;

public class MovieService : IMovieService
{
    private readonly MoviesContext db;

    public MovieService(MoviesContext db)
    {
        this.db = db;
    }

    public async Task<Movie?> GetMovieById(int id)
    {
        return await db.Movies.FindAsync(id);
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
        var movies = await db.Movies.ToListAsync();
        return movies;
    }
}
