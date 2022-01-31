using Core.Interfaces;
using ForterTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForterTask.Controllers;

public class CryptoController : ControllerBase
{
    private readonly ICryptoService _service;
    private readonly ILogger<CryptoController> _logger;

    public CryptoController(ICryptoService service, ILogger<CryptoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("SearchPerformance")]
    public async Task<IActionResult> SearchPerformance(
        [FromBody] SearchRequest request)
    {
        //The service returns it modelled, as per the instruction I return a key value mapping
        try
        {
            var result = await _service.GetPerformanceAsync(request.Symbols, request.Date);
            return Ok(result.Coins.ToDictionary(
                item => item.Symbol,
                item => item.Percentage.ToString("N") + "%"));
        }
        //This is not ideal -> It should be mapped correctly to a Result object. Due to time constraint I opted for exception handling
        catch (ArgumentException e)
        {
            return BadRequest("Invalid Request");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get Crypto Performance for {@request}", request);
            return Problem("Unable to get Crypto Performance");
        }
    }
}