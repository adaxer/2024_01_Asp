using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HelloRazor.Models;
using HelloRazor.Interfaces;

namespace HelloRazor.Pages.Movies;

public class CreateModel : PageModel
{
    private readonly IMovieService service;

    public CreateModel(IMovieService service)
    {
        this.service = service;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Movie Movie { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.SaveMovie(Movie);

        return RedirectToPage("./Index");
    }
}
