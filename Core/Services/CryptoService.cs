using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Providers;

namespace Core.Services;

public class CryptoService : ICryptoService
{
    private readonly ICryptoProvider _provider;

    public CryptoService(ICryptoProvider provider) => 
        _provider = provider;

    public async Task<IEnumerable<CoinPerformanceResponse>> GetPerformanceAsync(
        IEnumerable<string> symbols,
        DateTimeOffset date)
    {
        //TODO - Validation
        var result =
            await _provider.GetPerformanceAsync(new CoinRequest(symbols, date)) as List<CoinPerformanceResponse>;
        if (!result.Any())
        {
            //TODO - Return invalid
            throw new InvalidOperationException("Unable to find any coins");
        }

        return result;
    }
}