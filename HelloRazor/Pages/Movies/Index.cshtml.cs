using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HelloRazor.Models;

namespace HelloRazor.Pages.Movies;

public class IndexModel : PageModel
{
    private readonly HelloRazor.Data.MoviesContext _context;

    public IndexModel(HelloRazor.Data.MoviesContext context)
    {
        _context = context;
    }

    public IList<Movie> Movie { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Movie = await _context.Movies.ToListAsync();
    }
}
