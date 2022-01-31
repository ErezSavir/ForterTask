using Core.Entities;

namespace Core.Builders;

public class CoinBuilder : ICoinBuilder
{
    public Coin Build(string symbol, decimal percentage) =>
        new()
        {
            Symbol = symbol,
            Percentage = percentage
        };
}