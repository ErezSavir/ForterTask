using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Core.Entities.Requests;
using FluentAssertions;
using Infrastructure.Adapters;
using Infrastructure.ExternalServices;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Adapters;

public class CryptoAdapterTests
{
    private readonly Mock<ICryptoCompareClient> _clientMock;
    private readonly CryptoAdapter _adapter;
    private readonly Fixture _fixture = new();

    public CryptoAdapterTests()
    {
        _clientMock = new Mock<ICryptoCompareClient>();
        _adapter = new CryptoAdapter(Mock.Of<ILogger<CryptoAdapter>>(), _clientMock.Object);
    }

    [Fact]
    public async Task GetPerformanceAsync_InvalidRequest() =>
        await _adapter.Invoking(call => call.GetPerformanceAsync(null))
            .Should()
            .ThrowAsync<ArgumentException>();

    [Fact]
    public async Task GetPerformanceAsync_Converts_Epoch_Correctly()
    {
        var date = new DateTimeOffset(2022, 01, 01, 00, 00, 00, 00, TimeSpan.Zero);
        var epochDate = date.ToUnixTimeSeconds();
        var request = new CoinRequest(new List<string> {"BTC"}, date);

        _clientMock.Setup(call => call.GetHistoricDataAsync(It.IsAny<string>(), It.IsAny<long>()))
            .ReturnsAsync(_fixture.Create<CryptoRate>());

        await _adapter.GetPerformanceAsync(request);

        _clientMock.Verify(call => call.GetHistoricDataAsync("BTC", epochDate), Times.Once);
    }

    [Fact]
    public async Task GetPerformanceAsync_Calls_Twice_To_Client()
    {
        var request = new CoinRequest(new List<string> {"BTC"}, DateTimeOffset.Now);

        _clientMock.Setup(call => call.GetHistoricDataAsync(It.IsAny<string>(), It.IsAny<long>()))
            .ReturnsAsync(_fixture.Create<CryptoRate>());

        await _adapter.GetPerformanceAsync(request);

        _clientMock.Verify(call => call.GetHistoricDataAsync(It.IsAny<string>(), It.IsAny<long>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetPerformanceAsync_Returns_Correctly()
    {
        var request = new CoinRequest(new List<string> {"BTC"}, DateTimeOffset.Now);

        var historicRate = _fixture.Create<CryptoRate>();
        var currentRate = _fixture.Create<CryptoRate>();

        _clientMock.SetupSequence(call => call.GetHistoricDataAsync(It.IsAny<string>(), It.IsAny<long>()))
            .ReturnsAsync(historicRate)
            .ReturnsAsync(currentRate);

        var result = await _adapter.GetPerformanceAsync(request);

        
        _clientMock.Verify(call => call.GetHistoricDataAsync(It.IsAny<string>(), It.IsAny<long>()), Times.Exactly(2));
        result.Should().HaveCount(1);
        result.FirstOrDefault().Symbol.Should().Be("BTC");
        result.FirstOrDefault().CurrentRate.Should().Be(currentRate.Rate);
        result.FirstOrDefault().HistoricRate.Should().Be(historicRate.Rate);
    }
}