using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Builders;
using Core.Entities;
using Core.Entities.Requests;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Providers;
using Core.Services;
using Core.Validators;
using FluentAssertions;
using Moq;

namespace UnitTests.Services;

using Xunit;

public class CryptoServiceTests
{
    private readonly Mock<ICryptoProvider> _providerMock;
    private readonly Mock<IPerformanceRequestValidator> _validatorMock;

    private readonly ICryptoService _service;

    public CryptoServiceTests()
    {
        _providerMock = new Mock<ICryptoProvider>();
        _validatorMock = new Mock<IPerformanceRequestValidator>();
        _service = new CryptoService(_providerMock.Object, new PercentageHelper(), _validatorMock.Object, new CoinBuilder());
    }

    [Fact]
    public async Task GetPerformanceAsync_Invalid_Request()
    {
        _validatorMock.Setup(call => call.Validate(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTimeOffset>()))
            .Returns(false);

        await _service.Invoking(call => call.GetPerformanceAsync(new List<string>(), DateTimeOffset.Now)).Should()
            .ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetPerformanceAsync_Provider_Doesnt_Return_Data()
    {
        _validatorMock.Setup(call => call.Validate(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTimeOffset>()))
            .Returns(true);

        _providerMock.Setup(call => call.GetPerformanceAsync(It.IsAny<CoinRequest>()))
            .ReturnsAsync(new List<CoinPerformanceResponse>());

        await _service.Invoking(call => call.GetPerformanceAsync(new List<string>(), DateTimeOffset.Now)).Should()
            .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetPerformanceAsync_GetPercentage_Correctly()
    {
        _validatorMock.Setup(call => call.Validate(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTimeOffset>()))
            .Returns(true);

        var performanceResult = new List<CoinPerformanceResponse>
        {
            new()
            {
                Symbol = "AAA",
                CurrentRate = 100,
                HistoricRate = 50
            }
        };
        _providerMock.Setup(call => call.GetPerformanceAsync(It.IsAny<CoinRequest>()))
            .ReturnsAsync(performanceResult);

        var result = await _service.GetPerformanceAsync(new List<string>(), DateTimeOffset.Now);
        result.Coins.Should().HaveCount(1);
        result.Coins.First().Percentage.Should().Be(100);
    }
    
    [Fact]
    public async Task GetPerformanceAsync_OrderCorrectly()
    {
        _validatorMock.Setup(call => call.Validate(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTimeOffset>()))
            .Returns(true);

        var performanceResult = new List<CoinPerformanceResponse>
        {
            new()
            {
                Symbol = "AAA",
                CurrentRate = 100,
                HistoricRate = 50
            },
            new()
            {
                Symbol = "BBB",
                CurrentRate = 100,
                HistoricRate = 80
            }
        };
        _providerMock.Setup(call => call.GetPerformanceAsync(It.IsAny<CoinRequest>()))
            .ReturnsAsync(performanceResult);

        var result = await _service.GetPerformanceAsync(new List<string>(), DateTimeOffset.Now);
        result.Coins.Should().HaveCount(2);
        result.Coins[0].Symbol.Should().Be("AAA");
        result.Coins[1].Symbol.Should().Be("BBB");
    }
}