using NotesService.API;

// Builds a host and runs the "API" service
var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<NotesWorkerService>();

var host = builder.Build();

host.Run();
