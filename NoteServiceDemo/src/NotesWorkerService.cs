namespace NotesService.API;

public class NotesWorkerService : BackgroundService
{
    private readonly ILogger<NotesWorkerService> _logger;

    public NotesWorkerService(ILogger<NotesWorkerService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
