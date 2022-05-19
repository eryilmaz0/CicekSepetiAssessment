﻿using BasketService.Domain.Entity;

namespace BasketService.Application.Repository;

public interface IBasketRepository
{
    public Task<Basket> FindAsync(Func<Basket, bool> filter);
    public Task<bool> UpdateBasketAsync(Basket basket);
}