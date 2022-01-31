using Core.Builders;
using Core.Entities;
using Core.Entities.Requests;
using Core.Entities.Response;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Providers;
using Core.Validators;

namespace Core.Services;

public class CryptoService : ICryptoService
{
    private readonly ICryptoProvider _provider;
    private readonly IPercentageHelper _percentageHelper;
    private readonly IPerformanceRequestValidator _validator;
    private readonly ICoinBuilder _builder;

    public CryptoService(
        ICryptoProvider provider,
        IPercentageHelper percentageHelper,
        IPerformanceRequestValidator validator,
        ICoinBuilder builder)
    {
        _provider = provider;
        _percentageHelper = percentageHelper;
        _validator = validator;
        _builder = builder;
    }

    public async Task<CoinResponse> GetPerformanceAsync(
        IEnumerable<string> symbols,
        DateTimeOffset date)
    {
        if (!_validator.Validate(symbols, date))
            throw new ArgumentException("Invalid Get Performance request");

        var coinsPerformance = await _provider.GetPerformanceAsync(new CoinRequest(symbols, date))
            as List<CoinPerformanceResponse>;

        if (coinsPerformance != null && !coinsPerformance.Any())
            throw new InvalidOperationException("Unable to find any coins");

        var coinsOrderedList = coinsPerformance!.Select(coin =>
                _builder.Build(
                    coin.Symbol,
                    _percentageHelper.GetPercentageIncrease(coin.HistoricRate, coin.CurrentRate)))
            .OrderByDescending(t => t.Percentage)
            .ToList();

        return new CoinResponse {Coins = coinsOrderedList};
    }
}