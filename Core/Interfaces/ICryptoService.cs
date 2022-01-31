using Core.Entities;

namespace Core.Interfaces;

public interface ICryptoService
{
    Task<IEnumerable<CoinPerformanceResponse>> GetPerformanceAsync(IEnumerable<string> symbols, DateTimeOffset date);
}