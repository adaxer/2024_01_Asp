using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HelloRazor.Models;
using HelloRazor.Interfaces;

namespace HelloRazor.Pages.Movies;

public class DeleteModel : PageModel
{
    private readonly IMovieService service;

    public DeleteModel(IMovieService service)
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
        else
        {
            Movie = movie;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await service.RemoveMovieById(id.Value);
        return RedirectToPage("./Index");
    }
}
