using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    string xmlDocFilename = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
    // TODO: Add Security at some point later | AddSecurityDefinition/Requirement

    c.IncludeXmlComments(xmlDocFilename);
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Notes Service API (DEMO)",
        Version = "v1", // TODO make a proper version handling https://www.youtube.com/watch?v=8Asq7ymF1R8
        Description = "DEMO API for note storage.",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//TODO: might allow in production
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //c.RoutePrefix = "api/documentation"; // TODO in Production only
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// TODO: hosting https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-2.1&tabs=aspnetcore2x