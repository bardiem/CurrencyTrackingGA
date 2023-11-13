namespace CurrencyTrackingGA.Models;

public record CurrencyExchangeToUahModel
{
    public int R030 { get; set; }
    public string Txt { get; set; }
    public decimal Rate { get; set; }
    public string Cc { get; set; }
    public string ExchangeDate { get; set; }
}