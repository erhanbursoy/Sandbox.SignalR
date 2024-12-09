namespace SignalR.Lib
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class PrintJobService : IHostedService
    {
        private readonly ILogger<PrintJobService> _logger;
        private readonly PrintHubClient _printHubClient;

        public PrintJobService(ILogger<PrintJobService> logger, PrintHubClient printHubClient)
        {
            _logger = logger;
            _printHubClient = printHubClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PrintJobService is starting.");
            await _printHubClient.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PrintJobService is stopping.");
            await _printHubClient.StopAsync();
        }
    }
}
