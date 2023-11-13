namespace CurrencyTrackingGA.Models;

public record UahToUsdModel
{
    public decimal UsdRate { get; set; }
    public string Exchange { get; set; }
    public DateTime RateDateTime { get; set; }
}
