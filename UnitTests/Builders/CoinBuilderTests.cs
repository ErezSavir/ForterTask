using Core.Builders;
using Core.Entities;
using FluentAssertions;
using Xunit;

namespace UnitTests.Builders;

public class CoinBuilderTests
{
    private readonly ICoinBuilder _builder;

    public CoinBuilderTests()
    {
        _builder = new CoinBuilder();
    }

    [Fact]
    public void Builds_Correct_Entity()
    {
        var result = _builder.Build("BTC", 10);
        result.Should().BeOfType<Coin>();
        result.Symbol.Should().Be("BTC");
        result.Percentage.Should().Be(10);
    }
}