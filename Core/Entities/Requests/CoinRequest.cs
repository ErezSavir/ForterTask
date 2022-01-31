namespace Core.Entities.Requests;

public class CoinRequest
{
    public IEnumerable<string> Symbols { get; set; }
    public DateTimeOffset Date { get; set; }

    public CoinRequest(IEnumerable<string> symbols, DateTimeOffset date)
    {
        Date = date;
        Symbols = symbols;
    }
}