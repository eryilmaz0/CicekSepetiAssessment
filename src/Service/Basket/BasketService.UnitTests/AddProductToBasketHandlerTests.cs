using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BasketService.Application.Handler;
using BasketService.Application.Model;
using BasketService.Application.Proxy;
using BasketService.Application.Repository;
using BasketService.Application.Service;
using BasketService.Domain.Entity;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BasketService.UnitTests;

public class AddProductToBasketHandlerTests
{
    [Test]
    public async Task AddProductToBasket_WithNotValidUser_ShouldReturnUserNotValidError()
    {
        //Arrange
        AddProductToBasketRequest request = new()
        {
            Product = new()
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductImage = "Image.png",
                ProductPrice = 99.99M
            },
            Quantity = 10
        };
        
        var mockBasketRepository = new Mock<IBasketRepository>();
        var mockAuthService = new Mock<IAuthService>();
        var mockStockProxy = new Mock<IStockProxy>();

        mockAuthService.Setup(x => x.GetAuthenticatedUser()).Returns(default(AuthenticatedUser));
        AddProductToBasketRequestHandler handler = new(mockBasketRepository.Object, mockAuthService.Object, mockStockProxy.Object);
        
        //Act
        var result = await handler.Handle(request, new CancellationToken());
        
        //Assert
        mockAuthService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        result.IsSuccess.Should().BeFalse();
        result.ResultMessage.Should().Be("Authenticated User Not Valid.");
    }
    
    
    [Test]
    public async Task AddProductToBasket_WithNotAvailableStock_ShouldReturnStockIsNotValidError()
    {
        //Arrange
        AddProductToBasketRequest request = new()
        {
            Product = new()
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductImage = "Image.png",
                ProductPrice = 99.99M
            },
            Quantity = 10
        };

        AuthenticatedUser user = new()
        {
            UserId = Guid.NewGuid().ToString(),
            Name = "Eren",
            LastName = "Yılmaz",
            Email = "eryilmaz0@hotmail.com"
        };

        var mockBasketRepository = new Mock<IBasketRepository>();
        var mockAuthService = new Mock<IAuthService>();
        var mockStockProxy = new Mock<IStockProxy>();

        mockAuthService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        mockStockProxy.Setup(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>())).ReturnsAsync(false);
        AddProductToBasketRequestHandler handler = new(mockBasketRepository.Object, mockAuthService.Object, mockStockProxy.Object);
        
        //Act
        var result = await handler.Handle(request, new CancellationToken());
        
        //Assert
        mockAuthService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        mockStockProxy.Verify(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>()), Times.Once);
        result.IsSuccess.Should().BeFalse();
        result.ResultMessage.Should().Be("Stock Is Not Available for This Product.");
    }


    [Test]
    public async Task AddProductToBasket_WithNotFoundedBasket_ShouldReturnBasketNotFoundError()
    {
        //Arrange
        AddProductToBasketRequest request = new()
        {
            Product = new()
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductImage = "Image.png",
                ProductPrice = 99.99M
            },
            Quantity = 10
        };
    
        AuthenticatedUser user = new()
        {
            UserId = Guid.NewGuid().ToString(),
            Name = "Eren",
            LastName = "Yılmaz",
            Email = "eryilmaz0@hotmail.com"
        };
    
        var mockBasketRepository = new Mock<IBasketRepository>();
        var mockAuthService = new Mock<IAuthService>();
        var mockStockProxy = new Mock<IStockProxy>();
    
        mockAuthService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        mockStockProxy.Setup(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>())).ReturnsAsync(true);
        mockBasketRepository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>())).ReturnsAsync(default(Basket));
        AddProductToBasketRequestHandler handler = new(mockBasketRepository.Object, mockAuthService.Object, mockStockProxy.Object);
        
        //Act
        var result = await handler.Handle(request, new CancellationToken());
        
        //Assert
        mockAuthService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        mockStockProxy.Verify(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>()), Times.Once);
        mockBasketRepository.Verify(x => x.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>()));
        result.IsSuccess.Should().BeFalse();
        result.ResultMessage.Should().Be("Basket Not Found.");
    }
    
    
    [Test]
    public async Task AddProductToBasket_WithUpdateBasketError_ShouldReturnBasketNotUpdatedError()
    {
        //Arrange
        AddProductToBasketRequest request = new()
        {
            Product = new()
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductImage = "Image.png",
                ProductPrice = 99.99M
            },
            Quantity = 10
        };
    
        AuthenticatedUser user = new()
        {
            UserId = Guid.NewGuid().ToString(),
            Name = "Eren",
            LastName = "Yılmaz",
            Email = "eryilmaz0@hotmail.com"
        };

        Basket basket = new()
        {
            UserEmail = "eryilmaz0@hotmail.com",
            BasketItems = new()
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = 1,
                    ProductName = "Product 1",
                    ProductImage = "Product1.jpg",
                    ProductPrice = 99.99M,
                    DiscountCode = "Discount15",
                    DiscountRate = 15,
                    Quantity = 5,
                    LastUpdatedTime = DateTime.Now
                }
            }
        };
    
        var mockBasketRepository = new Mock<IBasketRepository>();
        var mockAuthService = new Mock<IAuthService>();
        var mockStockProxy = new Mock<IStockProxy>();
    
        mockAuthService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        mockStockProxy.Setup(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>())).ReturnsAsync(true);
        mockBasketRepository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>())).ReturnsAsync(basket);
        mockBasketRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<Basket>())).ReturnsAsync(false);
        AddProductToBasketRequestHandler handler = new(mockBasketRepository.Object, mockAuthService.Object, mockStockProxy.Object);
        
        //Act
        var result = await handler.Handle(request, new CancellationToken());
        
        //Assert
        mockAuthService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        mockStockProxy.Verify(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>()), Times.Once);
        mockBasketRepository.Verify(x => x.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>()));
        mockBasketRepository.Verify(x => x.UpdateBasketAsync(It.IsAny<Basket>()), Times.Once);
        result.IsSuccess.Should().BeFalse();
        result.ResultMessage.Should().Be("Basket Not Updated.");
    }


    [Test]
    public async Task AddProductToBasket_WithNonExistingProduct_ShoulBeAddedNewItemIntoBasket()
    {
        //Arrange
        AddProductToBasketRequest request = new()
        {
            Product = new()
            {
                ProductId = 2,
                ProductName = "Product 2",
                ProductImage = "Image.png",
                ProductPrice = 99.99M
            },
            Quantity = 10
        };
    
        AuthenticatedUser user = new()
        {
            UserId = Guid.NewGuid().ToString(),
            Name = "Eren",
            LastName = "Yılmaz",
            Email = "eryilmaz0@hotmail.com"
        };

        Basket basket = new()
        {
            UserEmail = "eryilmaz0@hotmail.com",
            BasketItems = new()
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = 1,
                    ProductName = "Product 1",
                    ProductImage = "Product1.jpg",
                    ProductPrice = 99.99M,
                    DiscountCode = "Discount15",
                    DiscountRate = 15,
                    Quantity = 5,
                    LastUpdatedTime = DateTime.Now
                }
            }
        };
    
        var mockBasketRepository = new Mock<IBasketRepository>();
        var mockAuthService = new Mock<IAuthService>();
        var mockStockProxy = new Mock<IStockProxy>();
    
        mockAuthService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        mockStockProxy.Setup(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>())).ReturnsAsync(true);
        mockBasketRepository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>())).ReturnsAsync(basket);
        mockBasketRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<Basket>())).ReturnsAsync(true);
        AddProductToBasketRequestHandler handler = new(mockBasketRepository.Object, mockAuthService.Object, mockStockProxy.Object);
        
        //Act
        var result = await handler.Handle(request, new CancellationToken());
        
        //Assert
        mockAuthService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        mockStockProxy.Verify(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>()), Times.Once);
        mockBasketRepository.Verify(x => x.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>()));
        mockBasketRepository.Verify(x => x.UpdateBasketAsync(It.IsAny<Basket>()), Times.Once);
        result.IsSuccess.Should().BeTrue();
        result.ResultMessage.Should().Be("Product Added To Basket.");
        
        //Verifying the Basket object
        basket.BasketItems.Should().HaveCount(2);
        basket.BasketItems.Should().Contain(x => x.ProductId.Equals(2));
    }
    
    
    [Test]
    public async Task AddProductToBasket_WithExistingProduct_ShouldBeIncreasedProductQuantity()
    {
        //Arrange
        AddProductToBasketRequest request = new()
        {
            Product = new()
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductImage = "Product1.jpg",
                ProductPrice = 99.99M,
                DiscountCode = "Discount15",
                DiscountRate = 15,
            },
            Quantity = 10
        };
    
        AuthenticatedUser user = new()
        {
            UserId = Guid.NewGuid().ToString(),
            Name = "Eren",
            LastName = "Yılmaz",
            Email = "eryilmaz0@hotmail.com"
        };

        Basket basket = new()
        {
            UserEmail = "eryilmaz0@hotmail.com",
            BasketItems = new()
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = 1,
                    ProductName = "Product 1",
                    ProductImage = "Product1.jpg",
                    ProductPrice = 99.99M,
                    DiscountCode = "Discount15",
                    DiscountRate = 15,
                    Quantity = 5,
                    LastUpdatedTime = DateTime.Now
                }
            }
        };
    
        var mockBasketRepository = new Mock<IBasketRepository>();
        var mockAuthService = new Mock<IAuthService>();
        var mockStockProxy = new Mock<IStockProxy>();
    
        mockAuthService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        mockStockProxy.Setup(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>())).ReturnsAsync(true);
        mockBasketRepository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>())).ReturnsAsync(basket);
        mockBasketRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<Basket>())).ReturnsAsync(true);
        AddProductToBasketRequestHandler handler = new(mockBasketRepository.Object, mockAuthService.Object, mockStockProxy.Object);
        
        //Act
        var result = await handler.Handle(request, new CancellationToken());
        
        //Assert
        mockAuthService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        mockStockProxy.Verify(x => x.IsStockAvailableAsync(It.IsAny<IsStockAvailableRequest>()), Times.Once);
        mockBasketRepository.Verify(x => x.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>()));
        mockBasketRepository.Verify(x => x.UpdateBasketAsync(It.IsAny<Basket>()), Times.Once);
        result.IsSuccess.Should().BeTrue();
        result.ResultMessage.Should().Be("Product Added To Basket.");
        
        //Verifying the Basket object
        basket.BasketItems.Should().HaveCount(1);
        basket.BasketItems.Should().Contain(x => x.ProductId.Equals(1));
        basket.BasketItems.First(x => x.ProductId.Equals(1)).Quantity.Should().Be(15);
    }
}