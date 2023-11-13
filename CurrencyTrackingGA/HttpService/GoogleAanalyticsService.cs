using CurrencyTrackingGA.Models;
using CurrencyTrackingGA.Settings;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace CurrencyTrackingGA.HttpService;

public class GoogleAanalyticsService
{
    private const string ClientId = "107111089712057392633";
    private const string EventName = "UahUsdRate";

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private readonly GoogleAnalyticsSettings _googleAnalyticsSettings;

    public GoogleAanalyticsService(IOptions<GoogleAnalyticsSettings> googleAnalyticsSettings, HttpClient httpClient)
    {
        _googleAnalyticsSettings = googleAnalyticsSettings.Value;
        _httpClient = httpClient;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }


    public async Task PostUahUsdRate(UahToUsdModel model)
    {
        try
        {
            var url = ConstructGAApiFullUrl();
            var json = ConstructJson(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await _httpClient.PostAsync(url, content);
            if (!res.IsSuccessStatusCode)
            {
                string msg = await res.Content.ReadAsStringAsync();
                Console.WriteLine(msg);
                throw new Exception(msg);
            }
        }
        catch
        {
            throw;
        }
    }

    private string ConstructJson(UahToUsdModel model)
    {
        var body = new GAPostModel { ClientId = ClientId };
        var gaEvent = new GAEventModel { Name = EventName, ExchangeRateModel = model };
        body.Events = new List<GAEventModel> { gaEvent };

        return JsonSerializer.Serialize(body, _options);
    }

    private string ConstructGAApiFullUrl()
    {
        return $"{_googleAnalyticsSettings.Url}?api_secret={_googleAnalyticsSettings.ApiSecret}&measurement_id={_googleAnalyticsSettings.MeasurementId}";
    }
}