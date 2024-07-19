namespace NotesService.API.DataAccess.Entities;

using System.ComponentModel.DataAnnotations;

internal sealed class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public ICollection<Note> Notes { get; set; }
}
