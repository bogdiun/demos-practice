namespace NotesService.API;

using System.Reflection;
using Asp.Versioning;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NotesService.API.Auth;
using NotesService.API.DataAccess;
using NotesService.API.Middlewares;
using NotesService.API.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // TODO: Authorization rules/policies not sure where this is supposed to go then
        builder.Services.AddAuthenticationServices(builder.Configuration);
        builder.Services.AddAuthorizationBuilder()
                        .AddPolicy("AdminAccess", p => p.RequireClaim("Admin"));

        // Swagger / OpenAPI things (inc openAPI security definitions/requirements)
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen();

        // API Versioning
        builder.Services.AddApiVersioning(c =>
                        {
                            c.DefaultApiVersion = new ApiVersion(1, 0);
                            c.AssumeDefaultVersionWhenUnspecified = true;
                            c.ReportApiVersions = true;
                        })
                        .AddMvc().AddApiExplorer(c =>
                        {
                            c.GroupNameFormat = "'v'VVV";
                            c.SubstituteApiVersionInUrl = true;
                        });

        builder.Services.AddControllers();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddDataPersistance(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            //TODO: might allow in production, add: c.RoutePrefix = "api/documentation"
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var version in app.DescribeApiVersions().Select(d => d.GroupName))
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
                    c.DisplayRequestDuration();
                }
            });


            await app.RunAuthDatabaseMigrationAsync();
            await app.RunDALDatabaseMigrationAsync();
        }

        // TODO: add RateLimiting

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}

// TODO: hosting https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-2.1&tabs=aspnetcore2x