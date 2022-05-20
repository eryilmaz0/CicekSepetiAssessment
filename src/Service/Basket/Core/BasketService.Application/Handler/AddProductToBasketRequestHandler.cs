using BasketService.Application.Model;
using BasketService.Application.Proxy;
using BasketService.Application.Repository;
using BasketService.Application.Service;
using BasketService.Domain.Entity;
using MediatR;
using MongoDB.Bson;

namespace BasketService.Application.Handler;

public class AddProductToBasketRequestHandler : IRequestHandler<AddProductToBasketRequest, AddProductToBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IAuthService _authService;
    private readonly IStockProxy _stockProxy;

    public AddProductToBasketRequestHandler(IBasketRepository basketRepository,IAuthService authService, IStockProxy stockProxy)
    {
        _basketRepository = basketRepository;
        _authService = authService;
        _stockProxy = stockProxy;
    }

    public async Task<AddProductToBasketResponse> Handle(AddProductToBasketRequest request, CancellationToken cancellationToken)
    {
        if (!IsAuthenticatedUserValid(out AuthenticatedUser authenticatedUser))
            return new() { IsSuccess = false, ResultMessage = "Authenticated User Not Valid." };

        if (!await IsStockAvailable(request.Product.ProductId, request.Quantity))
            return new() { IsSuccess = false, ResultMessage = "Stock Is Not Available for This Product." };

        var basket = await GetBasketForAuthenticatedUser(authenticatedUser.Email);

        if (basket is null)
        {
            return new() { IsSuccess = false, ResultMessage = "Basket Not Found." };
        }

        var result = await AddProductToBasket(basket, request.Product, request.Quantity);

        if (!result)
            return new() { IsSuccess = false, ResultMessage = "Basket Not Updated." };
        
        return new() { IsSuccess = true, ResultMessage = "Product Added To Basket." };
    }


    private bool IsAuthenticatedUserValid(out AuthenticatedUser user)
    {
        var authenticatedUser = this._authService.GetAuthenticatedUser();

        if (authenticatedUser is not null)
        {
            user = authenticatedUser;
            return true;
        }

        user = default(AuthenticatedUser);
        return false;
    }


    private async Task<bool> IsStockAvailable(int productId, int quantity)
    {
        var requestModel = new IsStockAvailableRequest()
        {
            ProductId = productId,
            Quantity = quantity
        };

        return await _stockProxy.IsStockAvailableAsync(requestModel);
    }

    private async Task<Basket> GetBasketForAuthenticatedUser(string userEmail)
    {
        return await _basketRepository.FindAsync(basket => basket.UserEmail.Equals(userEmail));
    }

    

    private async Task<bool> AddProductToBasket(Basket basket, ProductModel product, int quantity)
    {
        var productInBasket = basket.BasketItems
                                .FirstOrDefault(basketItem => basketItem.ProductId.Equals(product.ProductId));

        if (productInBasket is null)
        {
            BasketItem newBasketItem = new()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                ProductPrice = product.ProductPrice,
                LastUpdatedTime = DateTime.Now,
                DiscountRate = product.DiscountRate,
                DiscountCode = product.DiscountCode,
                Quantity = quantity
            };
            
            basket.BasketItems.Add(newBasketItem);
        }

        else
        {
            productInBasket.Quantity += quantity;
            productInBasket.ProductName = product.ProductName;
            productInBasket.ProductImage = product.ProductImage;
            productInBasket.ProductPrice = product.ProductPrice;
            productInBasket.LastUpdatedTime = DateTime.Now;
            productInBasket.DiscountRate = product.DiscountRate;
            productInBasket.DiscountCode = product.DiscountCode;
        }

        return await this._basketRepository.UpdateBasketAsync(basket);
    }
}