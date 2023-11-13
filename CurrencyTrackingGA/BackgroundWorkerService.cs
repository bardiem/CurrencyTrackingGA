using CurrencyTrackingGA.HttpService;

public class BackgroundWorkerService : BackgroundService
{
    private readonly ILogger<BackgroundWorkerService> _logger;
    private readonly PeriodicTimer _twentyMinuteTimer = new PeriodicTimer(TimeSpan.FromHours(10));
    private readonly GoogleAanalyticsService _googleAnalyticsService;
    private readonly CurrencyService _currencyService;

    public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger, GoogleAanalyticsService googleAnalyticsService, CurrencyService currencyService)
    {
        _googleAnalyticsService = googleAnalyticsService;
        _currencyService = currencyService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _twentyMinuteTimer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                var model = await _currencyService.GetCurrentUsdCourseAsync();
                await _googleAnalyticsService.PostUahUsdRate(model);

            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            

            await Task.Delay(10000, stoppingToken);
        }
    }
}