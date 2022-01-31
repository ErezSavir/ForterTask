using Core.Helpers;
using FluentAssertions;
using Xunit;

namespace UnitTests.Helpers;

public class PercentageHelperTests
{
    private readonly IPercentageHelper _helper;

    public PercentageHelperTests() => _helper = new PercentageHelper();

    [Theory]
    [InlineData(50, 100, 100)]
    [InlineData(1, 10, 900)]
    [InlineData(10, 1, -90)]
    [InlineData(10, 10, 0)]
    [InlineData(10000, 20000, 100)]
    [InlineData(100000000, 200000000, 100)]
    public void Calculate_Percentage_Correctly(decimal original, decimal current, decimal result) => 
        _helper.GetPercentageIncrease(original, current).Should().Be(result);
}