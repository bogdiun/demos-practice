namespace NotesService.API.DataAccess;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotesService.API.Abstractions;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<DataContext>(c =>
        {
            c.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            //c.UseInMemoryDatabase(configuration.GetConnectionString("InMemDB"));
        });
        services.AddScoped<INotesRepository, NotesRepository>();
        services.AddScoped<IMediaTypeRepository, MediaTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }

    public static async Task UseDatabaseMigrationAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        await dbContext.Database.MigrateAsync();
    }
}
