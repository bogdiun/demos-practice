namespace NotesService.API;

using System.Reflection;
using Asp.Versioning;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NotesService.API.Common;
using NotesService.API.Common.DTO.Request;
using NotesService.API.DataAccess;
using NotesService.API.Swagger;
using NotesService.API.Validators;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // TODO: DBContext config should be job of DataAccess layer?
        builder.Services.AddDbContextPool<DataContext>(c =>
        {
            //c.UseInMemoryDatabase(builder.Configuration.GetConnectionString("InMemDB"));
            c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddScoped<INotesRepository, NotesRepository>();
        builder.Services.AddScoped<IMediaTypeRepository, MediaTypeRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

        builder.Services.AddControllers();
        builder.Services.AddScoped<IValidator<NotePostRequest>, NotePostRequestValidator>();
        //builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            string xmlDocFilename = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
            c.IncludeXmlComments(xmlDocFilename);
            c.OperationFilter<SwaggerDefaultValues>();
            // TODO: Add Security at some point later | AddSecurityDefinition/Requirement
        });
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.

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
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}

// TODO: hosting https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-2.1&tabs=aspnetcore2x