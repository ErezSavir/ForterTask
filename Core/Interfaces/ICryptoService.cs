using Core.Entities;
using Core.Entities.Response;

namespace Core.Interfaces;

public interface ICryptoService
{
    Task<CoinResponse> GetPerformanceAsync(IEnumerable<string> symbols, DateTimeOffset date);
}