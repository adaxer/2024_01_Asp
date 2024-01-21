using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using MovieLibrary.Models;
using MovieLibrary.Interfaces;

namespace MovieLibrary.Areas.Movies.Pages;

public class EditModel : PageModel
{
    private readonly IMovieService service;

    public EditModel(IMovieService service)
    {
        this.service = service;
    }

    [BindProperty]
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
        Movie = movie;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }


        try
        {
            await service.SaveMovie(Movie);
        }
        catch (Exception ex)
        {
            Trace.TraceError($"{ex}");
            return NotFound();
        }

        return RedirectToPage("./Index");
    }
}
