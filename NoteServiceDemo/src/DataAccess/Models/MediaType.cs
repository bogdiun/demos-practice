namespace NotesService.API.DataAccess.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MediaType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string TypeName { get; set; } = null!;
    // Word, Text,  Audio, Image, ??

    // navigational property
    public ICollection<Note> Notes { get; set; }
}