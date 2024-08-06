namespace NotesService.API.Swagger;

using System.Reflection;
using System.Text;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.OperationFilter<SwaggerDefaultValues>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        string xmlDocFilename = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
        options.IncludeXmlComments(xmlDocFilename);

        OpenApiSecurityScheme securityScheme = CreateSecurityScheme();

        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, Array.Empty<string>() },
        });
    }

    private static OpenApiSecurityScheme CreateSecurityScheme() => new()
    {
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "JWT Authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme,
        },
    };

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var text = new StringBuilder("API for note storage.");
        var info = new OpenApiInfo()
        {
            Title = "Notes Service API (DEMO)",
            Version = description.ApiVersion.ToString(),
            Contact = new OpenApiContact() { Name = "Raimond Bogdiun" },
            //License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
        };

        if (description.IsDeprecated)
        {
            text.Append(" This API version has been deprecated.");
        }

        info.Description = text.ToString();

        return info;
    }
}
