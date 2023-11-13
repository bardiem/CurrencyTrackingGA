using CurrencyTrackingGA.HttpService;
using CurrencyTrackingGA.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<GoogleAnalyticsSettings>(builder.Configuration.GetSection("GoogleAnalyticsSettings"));
builder.Services.Configure<NbuExchangeRateSettings>(builder.Configuration.GetSection("NBURateSettings"));
builder.Services.AddHttpClient<GoogleAanalyticsService>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
builder.Services.AddHttpClient<CurrencyService>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddControllers();
builder.Services.AddHostedService<BackgroundWorkerService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
