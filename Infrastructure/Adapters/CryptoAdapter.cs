using Core.Entities;
using Core.Interfaces.Providers;
using Infrastructure.ExternalServices;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Adapters;

public class CryptoAdapter : ICryptoProvider
{
    private readonly ILogger<CryptoAdapter> _logger;
    private readonly ICryptoCompareClient _client;

    public CryptoAdapter(ILogger<CryptoAdapter> logger, ICryptoCompareClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<IEnumerable<CoinPerformanceResponse>> GetPerformanceAsync(CoinRequest request)
    {
        if (request == null)
            throw new ArgumentException("Invalid request");

        var result = new List<CoinPerformanceResponse>();
        var requestedEpochTime = request.Date.ToUnixTimeSeconds();
        var currentEpochTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        foreach (var symbol in request.Symbols)
        {
            var historicRate = await _client.GetHistoricDataAsync(symbol, requestedEpochTime);
            var currentRate = await _client.GetHistoricDataAsync(symbol, currentEpochTime);
            if (historicRate == null || currentRate == null)
            {
                _logger.LogWarning("Unable to get rate for {symbol}", symbol);
                continue;
            }

            result.Add(new CoinPerformanceResponse
            {
                Symbol = symbol,
                PerformanceInPercentage =
                    ((currentRate.Rate - historicRate.Rate) / historicRate.Rate * 100).ToString("N") + "%"
            });
        }

        return result;
    }
}