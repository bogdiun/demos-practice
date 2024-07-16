namespace NotesService.API.DataAccess;

using Microsoft.EntityFrameworkCore;
using NotesService.API.DataAccess.Models;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Note> Notes { get; set; }
}
