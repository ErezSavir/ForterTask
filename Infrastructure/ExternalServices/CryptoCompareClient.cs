using Flurl;
using Flurl.Http;
using Infrastructure.Models;
using Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalServices;

public class CryptoCompareClient : ICryptoCompareClient
{
    private readonly CryptoApiSettings _settings;
    private readonly ILogger<CryptoCompareClient> _logger;

    public CryptoCompareClient(IOptions<CryptoApiSettings> settings, ILogger<CryptoCompareClient> logger)
    {
        _logger = logger;
        _settings = settings.Value;
    }


    public async Task<CryptoRate?> GetHistoricDataAsync(string symbol, long epochTime)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Invalid symbol");

        try
        {
            var result = await _settings.BaseUrl
                .SetQueryParams(new
                {
                    api_key = _settings.ApiKey,
                    fsym = symbol,
                    tsyms = _settings.ConvertToSymbol,
                    ts = epochTime
                })
                .GetStringAsync();

            return new CryptoRate
            {
                Rate = Newtonsoft.Json.Linq.JObject.Parse(result)[symbol]!.Value<decimal>("USD"),
                TimeStampEpoch = epochTime
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to fetch information");
            return null;
        }
    }
}