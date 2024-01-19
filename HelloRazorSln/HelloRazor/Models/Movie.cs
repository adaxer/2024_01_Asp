using System.ComponentModel.DataAnnotations;

namespace HelloRazor.Models;

public class Movie
{
    public int Id { get; set; }
    [Required(ErrorMessage ="Title is required")]
    public string Title { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    [Display(Name ="Date of Release")]
    public DateTime ReleaseDate { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }
}