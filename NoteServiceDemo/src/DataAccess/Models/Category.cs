namespace NotesService.API.DataAccess.Models;

using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    // navigational property
    public ICollection<Note> Notes { get; set; }
}
