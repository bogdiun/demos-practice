namespace NotesService.API.DataAccess.Entities;

using System.ComponentModel.DataAnnotations;

internal sealed class MediaType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string TypeName { get; set; } = null!;

    public ICollection<Note> Notes { get; set; }
}