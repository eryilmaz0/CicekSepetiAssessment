using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using StockService.Business;
using StockService.DataAccess;
using StockService.Entity.Dto;
using StockService.Entity.Entity;

namespace StockService.UnitTest;

public class StockServiceTests
{
    [Test]
    public async Task CheckStockAvailability_WithNonExistProductId_ShouldReturnStockNotFoundError()
    {
        CheckStockAvailabilityRequest request = new()
        {
            ProductId = 1,
            Quantity = 1
        };

        var mockStockRepository = new Mock<IStockRepository>();
        mockStockRepository.Setup(x => x.GetStockByProductIdAsync(It.IsAny<int>())).ReturnsAsync(default(Stock));
        Business.StockService stockService = new Business.StockService(mockStockRepository.Object);
        
        //Act
        var result = await stockService.CheckStockAvailability(request);
        
        //Assert
        mockStockRepository.Verify(x => x.GetStockByProductIdAsync(It.IsAny<int>()),Times.Once);
        result.IsAvailable.Should().BeFalse();
        result.ResultMessage.Should().Be("Stock Not Found.");
    }
    
    
    [Test]
    public async Task CheckStockAvailability_WithGreaterQuantity_ShouldReturnStockNotAvailableError()
    {
        CheckStockAvailabilityRequest request = new()
        {
            ProductId = 1,
            Quantity = 10
        };

        Stock stock = new()
        {
            ProductId = 1,
            TotalStocks = 1000,
            AvailableStocks = 5
        };

        var mockStockRepository = new Mock<IStockRepository>();
        mockStockRepository.Setup(x => x.GetStockByProductIdAsync(It.IsAny<int>())).ReturnsAsync(stock);
        Business.StockService stockService = new Business.StockService(mockStockRepository.Object);
        
        //Act
        var result = await stockService.CheckStockAvailability(request);
        
        //Assert
        mockStockRepository.Verify(x => x.GetStockByProductIdAsync(request.ProductId),Times.Once);
        result.IsAvailable.Should().BeFalse();
        result.ResultMessage.Should().Be("Stock Not Available.");
    }
    
    
    [Test]
    public async Task CheckStockAvailability_WithAvailableQuantity_ShouldReturnStockAvailable()
    {
        CheckStockAvailabilityRequest request = new()
        {
            ProductId = 1,
            Quantity = 10
        };

        Stock stock = new()
        {
            ProductId = 1,
            TotalStocks = 1000,
            AvailableStocks = 500
        };

        var mockStockRepository = new Mock<IStockRepository>();
        mockStockRepository.Setup(x => x.GetStockByProductIdAsync(It.IsAny<int>())).ReturnsAsync(stock);
        Business.StockService stockService = new Business.StockService(mockStockRepository.Object);
        
        //Act
        var result = await stockService.CheckStockAvailability(request);
        
        //Assert
        mockStockRepository.Verify(x => x.GetStockByProductIdAsync(request.ProductId),Times.Once);
        result.IsAvailable.Should().BeTrue();
        result.ResultMessage.Should().Be("Stock is Available.");
    }
}