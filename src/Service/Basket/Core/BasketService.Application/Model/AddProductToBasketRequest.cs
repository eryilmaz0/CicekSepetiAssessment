using MediatR;

namespace BasketService.Application.Model;

public class AddProductToBasketRequest : IRequest<AddProductToBasketResponse>
{
    public ProductModel Product { get; set; }
    public int Quantity { get; set; }
    
}