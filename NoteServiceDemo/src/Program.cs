using System.Reflection;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using NotesService.API.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddEndpointsApiExplorer();

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

builder.Services.AddSwaggerGen(c =>
{
    string xmlDocFilename = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
    c.IncludeXmlComments(xmlDocFilename);
    c.OperationFilter<SwaggerDefaultValues>();
    // TODO: Add Security at some point later | AddSecurityDefinition/Requirement
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    //TODO: might allow in production, add: c.RoutePrefix = "api/documentation"
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
            c.DisplayRequestDuration();
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// TODO: hosting https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-2.1&tabs=aspnetcore2x