using BasketService.Application.Model;
using BasketService.Infrastructure.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.API.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class BasketsController : Controller
{
   private readonly IMediator _mediator;

   public BasketsController(IMediator mediator)
   {
      _mediator = mediator;
   }

   [HttpPost]
   [Authorize]
   public async Task<ApiResponse<AddProductToBasketResponse>> AddProductToBasket([FromBody] AddProductToBasketRequest request)
   {
      var result = await this._mediator.Send(request);
      return new(result);
   }


    [HttpPost]
    [Authorize]
    public async Task<ApiResponse<GetBasketResponse>> GetBasket([FromQuery]GetBasketRequest request)
    {
        var result = await this._mediator.Send(request);
        return new(result);
    }
}