using Serilog;

namespace WorkerServiceCore
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public HttpClient client { get; set; }


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        { 
            client.Dispose();
            _logger.LogInformation("Service is close!!!!!");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.GetAsync("https://www.google.com");
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("All is well!! Website is up now");
                }
                else
                {
                    _logger.LogInformation("All is not well !! Website is down");
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}