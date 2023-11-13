using System.Text.Json.Serialization;

namespace CurrencyTrackingGA.Models;

public record GAEventModel
{
    public string Name { get; set; }
    [JsonPropertyName("params")]
    public UahToUsdModel ExchangeRateModel { get; set; }
}
