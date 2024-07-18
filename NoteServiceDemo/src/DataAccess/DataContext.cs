namespace NotesService.API.DataAccess;

using Microsoft.EntityFrameworkCore;
using NotesService.API.DataAccess.Models;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Note> Notes { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<MediaType> MediaTypes { get; set; }
}
