using Microsoft.EntityFrameworkCore;
using MovieLibrary.Models;

namespace MovieLibrary.Data;

public class MoviesContext : DbContext
{
    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = default!;
}
