namespace NotesService.API.DataAccess;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotesService.API.Abstractions;
using NotesService.API.DataAccess.Repositories;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDataPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<DataContext>(c => c.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<INotesRepository, NotesRepository>();
        services.AddScoped<IMediaTypeRepository, MediaTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }

    public static async Task RunDALDatabaseMigrationAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        await dbContext.Database.MigrateAsync();
    }
}
