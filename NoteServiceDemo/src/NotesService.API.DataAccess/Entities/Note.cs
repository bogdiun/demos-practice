﻿namespace NotesService.API.DataAccess.Entities;

using System.ComponentModel.DataAnnotations;

// TODO: add subnotes concept
// TODO: in case there are more entities that are associated with user we could also have a separate catalog of all user items
internal sealed class Note
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    required public string NoteKey { get; set; }

    required public string NoteValue { get; set; }

    required public DateTime LastAccess { get; set; }

    required public MediaType MediaType { get; set; }

    public ICollection<Category> Categories { get; set; }

    required public string UserId { get; set; }
}
