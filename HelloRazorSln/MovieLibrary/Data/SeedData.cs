﻿using MovieLibrary.Models;

namespace MovieLibrary.Data;

public class SeedData
{
    public static void CreateDummyData(MoviesContext db)
    {
        db.Database.EnsureCreated();

        if (db.Movies.Any())
        {
            return;
        }

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
        db.Movies.AddRange(movies);
        db.SaveChanges();
    }
}