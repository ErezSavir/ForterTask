namespace Core.Entities;

public class CoinPerformanceResponse
{
    public string Symbol { get; set; }
    public decimal HistoricRate { get; set; }
    public decimal CurrentRate { get; set; }
}