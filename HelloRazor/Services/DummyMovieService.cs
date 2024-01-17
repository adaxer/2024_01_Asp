using HelloRazor.Interfaces;
using HelloRazor.Models;
using System.Diagnostics;

namespace HelloRazor.Services;

public class DummyMovieService : IMovieService
{
    public Task<IEnumerable<Movie>> GetMovies()
    {
        Trace.WriteLine("Trace debug von DummyMovieService");
        Trace.TraceInformation("Trace information von DummyMovieService");
        Trace.TraceWarning("Trace warning von DummyMovieService");
        Trace.TraceError("Trace error von DummyMovieService");
        var movies = new List<Movie>
        {
              new Movie
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M
                },

                new Movie
                {
                    Title = "Ghostbusters ",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M
                },

                new Movie
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M
                },

                new Movie
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M
                }
        };
        return Task.FromResult(movies as IEnumerable<Movie>);
    }
}
