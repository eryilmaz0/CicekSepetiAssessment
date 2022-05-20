using BasketService.Application.Model;
using BasketService.Application.Repository;
using BasketService.Application.Service;
using BasketService.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Application.Handler
{
    public class GetBasketRequestHandler : IRequestHandler<GetBasketRequest, GetBasketResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IAuthService _authService;

        public GetBasketRequestHandler(IBasketRepository basketRepository, IAuthService authService)
        {
            _basketRepository = basketRepository;
            _authService = authService;
        }

        public async Task<GetBasketResponse> Handle(GetBasketRequest request, CancellationToken cancellationToken)
        {
            var currentUser = _authService.GetAuthenticatedUser();

            if (currentUser is null)
                return new() { IsSuccess = false, ResultMessage = "Authenticated User Not Valid." };

            var basket = await _basketRepository.FindAsync(x => x.UserEmail.Equals(currentUser.Email));

            if (basket is null)
                return new() { IsSuccess = false, ResultMessage = "Basket Not Found For Authenticated User." };

            return this.PrepareResponse(basket);
        }



        private GetBasketResponse PrepareResponse(Basket basket)
        {
            var response = new GetBasketResponse()
            {
                IsSuccess = true,
                ResultMessage = "Basket Fetched Successfully.",
                Basket = new()
                {
                    Id = basket.Id,
                    UserEmail = basket.UserEmail,
                    LastUpdatedTime = basket.LastUpdatedTime,
                    BasketItems = new()
                }
            };

            basket.BasketItems.ForEach(item =>
            {
                response.Basket.BasketItems.Add(new()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductImage = item.ProductImage,
                    ProductPrice = item.ProductPrice,
                    Quantity = item.Quantity,
                    DiscountCode = item.DiscountCode,
                    DiscountRate = item.DiscountRate,
                    LastUpdatedTime = item.LastUpdatedTime
                });
            });

            return response;
        }
    }
}
