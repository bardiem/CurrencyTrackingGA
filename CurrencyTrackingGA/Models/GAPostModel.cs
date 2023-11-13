using System.Text.Json.Serialization;

namespace CurrencyTrackingGA.Models;

public record GAPostModel
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }
    public IList<GAEventModel> Events { get; set; }

}
