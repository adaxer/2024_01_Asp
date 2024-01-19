using HelloRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloRazor.Data;

public class MoviesContext : DbContext
{
    public MoviesContext (DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = default!;
}
