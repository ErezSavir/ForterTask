using Core.Entities;

namespace Core.Builders;

public interface ICoinBuilder
{
    Coin Build(string symbol, decimal percentage);
}