﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieLibrary.Models;
using MovieLibrary.Interfaces;

namespace MovieLibrary.Areas.Movies.Pages;

public class DetailsModel : PageModel
{
    private readonly IMovieService service;

    public DetailsModel(IMovieService service)
    {
        this.service = service;
    }

    public Movie Movie { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await service.GetMovieById(id.Value);
        if (movie == null)
        {
            return NotFound();
        }
        else
        {
            Movie = movie;
        }
        return Page();
    }
}
