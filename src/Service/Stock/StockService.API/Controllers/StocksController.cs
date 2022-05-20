using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockService.API.ApiResponse;
using StockService.Business;
using StockService.Entity.Dto;

namespace StockService.API.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class StocksController : ControllerBase
{
    private readonly IStockService _stockService;

    public StocksController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ApiResponse<CheckStockAvailabilityResponse>> CheckStockAvailability([FromBody] CheckStockAvailabilityRequest request)
    {
        var result = await _stockService.CheckStockAvailability(request);
        return new(result);
    }


    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok("Stock Service Health Check Success.");
    }
}