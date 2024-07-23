namespace NotesService.API;

using System.Reflection;
using System.Text;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Authentication;
using NotesService.API.DataAccess;
using NotesService.API.Swagger;
using NotesService.API.Validators;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDataAccessLayer(builder.Configuration);

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.Key));

        builder.Services.AddAuthentication(o =>
                        {
                            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(o =>
                        {
                            o.SaveToken = true;
                            o.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                // TODO: figure out how to take it from configuration easily
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection(JwtSettings.Key + ":Secret").Value)),
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                RequireExpirationTime = false,
                                ValidateLifetime = true,
                            };
                        });
        // TODO: Add OAuth as an option?

        builder.Services.AddControllers();
        builder.Services.AddScoped<IValidator<NotePostRequest>, NotePostRequestValidator>();
        builder.Services.AddScoped<IValidator<MediaTypeRequest>, MediaTypeRequestValidator>();
        builder.Services.AddScoped<IValidator<CategoryRequest>, CategoryRequestValidator>();
        //builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            string xmlDocFilename = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
            c.IncludeXmlComments(xmlDocFilename);
            c.OperationFilter<SwaggerDefaultValues>();

            OpenApiSecurityScheme securityScheme = new()
            {
                Scheme = "Bearer",
                Description = "JWT Authorization header",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            };

            c.AddSecurityDefinition(securityScheme.Scheme, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() },
            });
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
        app.UseAuthentication();
        //app.UseAuthorization();

        app.MapControllers();

        await app.UseDatabaseMigrationAsync();
        await app.RunAsync();
    }
}

// TODO: hosting https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-2.1&tabs=aspnetcore2x