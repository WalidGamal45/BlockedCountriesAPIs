using BDAssignment.Application.Services;

namespace BDAssignment.Presentation.BackgroundServices
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly CountryBlockService _countryBlockService;
        private readonly ILogger<TemporalBlockCleanupService> _logger;

        public TemporalBlockCleanupService(
            CountryBlockService countryBlockService,
            ILogger<TemporalBlockCleanupService> logger)
        {
            _countryBlockService = countryBlockService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TemporalBlockCleanupService started...");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for expired temporary blocks...");

                _countryBlockService.RemoveExpiredBlocks();

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("TemporalBlockCleanupService stopped.");
        }
    }
}
