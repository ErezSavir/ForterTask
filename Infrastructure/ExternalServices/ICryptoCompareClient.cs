using Infrastructure.Models;

namespace Infrastructure.ExternalServices;

public interface ICryptoCompareClient
{
    Task<CryptoRate?> GetHistoricDataAsync(string symbol, long epochTime);
}