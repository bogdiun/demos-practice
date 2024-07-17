﻿namespace NotesService.API.DataAccess.Models;

using System.ComponentModel.DataAnnotations;

public class Note
{
    [Key]
    public int Id { get; set; }

    required public string NoteKey { get; set; }

    required public string NoteValue { get; set; }

    required public DateTime LastAccess { get; set; }

    required public string MediaType { get; set; }

    public string? Categories { get; set; }
}
