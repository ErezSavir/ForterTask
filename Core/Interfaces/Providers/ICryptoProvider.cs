using Core.Entities;
using Core.Entities.Requests;

namespace Core.Interfaces.Providers;

public interface ICryptoProvider
{
    Task<IEnumerable<CoinPerformanceResponse>> GetPerformanceAsync(CoinRequest request);
}