using Core.Entities;
using Core.Interfaces;
using ForterTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForterTask.Controllers;

public class CryptoController : ControllerBase
{
    private readonly ICryptoService _service;

    public CryptoController(ICryptoService service) => _service = service;

    [HttpPost("SearchPerformance")]
    public async Task<IEnumerable<CoinPerformanceResponse>> SearchPerformance(
        [FromBody] SearchRequest request)
    {
        //TODO - Validation

        //TODO - Mapping
        return await _service.GetPerformanceAsync(request.Symbols, request.Date);
    }
}