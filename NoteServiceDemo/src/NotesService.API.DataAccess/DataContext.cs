namespace NotesService.API.DataAccess;

using Microsoft.EntityFrameworkCore;
using NotesService.API.DataAccess.Entities;

internal sealed class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Note> Notes { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<MediaType> MediaTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TODO seed the DB with Data
    }
}
