using CurrencyTrackingGA.Models;
using CurrencyTrackingGA.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CurrencyTrackingGA.HttpService;

public class CurrencyService
{
    private readonly NbuExchangeRateSettings _nbuExchangeSettings;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public CurrencyService(IOptions<NbuExchangeRateSettings> nbuExchangeSettings, HttpClient httpClient)
    {
        _nbuExchangeSettings = nbuExchangeSettings.Value;
        _httpClient = httpClient;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<UahToUsdModel> GetCurrentUsdCourseAsync()
    {
        try
        {
            var res = await _httpClient.GetAsync(_nbuExchangeSettings.Url + "?" + _nbuExchangeSettings.DataFormat);
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadFromJsonAsync<IEnumerable<CurrencyExchangeToUahModel>>(_options);

                var usdRecord = content.First(c => c.Cc == "USD");
                var exchange = GetExchangeNumbers(usdRecord);
                var uahToUsd = new UahToUsdModel { UsdRate = usdRecord.Rate, RateDateTime = DateTime.UtcNow, Exchange = exchange };
                return uahToUsd;
            }
            else
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

    private string GetExchangeNumbers(CurrencyExchangeToUahModel model)
    {
        int usd = 1;
        var uah = model.Rate;
        while (uah < 1)
        {
            usd *= 10;
            uah *= 10;
        }
        return $"{uah} UAH for {usd} USD";
    }

}
