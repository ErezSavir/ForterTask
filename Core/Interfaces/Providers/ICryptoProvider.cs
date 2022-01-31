using Core.Entities;

namespace Core.Interfaces.Providers;

public interface ICryptoProvider
{
    Task<IEnumerable<CoinPerformanceResponse>> GetPerformanceAsync(CoinRequest request);
}