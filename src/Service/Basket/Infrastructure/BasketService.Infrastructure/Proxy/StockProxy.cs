using BasketService.Application.Model;
using BasketService.Application.Proxy;
using BasketService.Application.Service;
using BasketService.Infrastructure.ProxyModel;
using System.Net.Http;
using System.Net.Http.Json;

namespace BasketService.Infrastructure.Proxy;

public class StockProxy : IStockProxy
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IAuthService _authService;

    public StockProxy(IHttpClientFactory clientFactory, IAuthService authService)
    {
        _clientFactory = clientFactory;
        _authService = authService; 
    }


    public async Task<bool> IsStockAvailableAsync(IsStockAvailableRequest request)
    {
        var httpClient = this._clientFactory.CreateClient("StockService");
        string currentToken = this._authService.GetCurrentToken();
        httpClient.DefaultRequestHeaders.Add("Authorization", currentToken);

        var requestModel = new CheckStockAvailabilityRequestModel() { ProductId = request.ProductId, Quantity = request.Quantity };
        var result = await httpClient.PostAsJsonAsync<CheckStockAvailabilityRequestModel>(string.Empty, requestModel);

        if (result is null || !result.IsSuccessStatusCode)
            return false;

        var response = await result.Content.ReadFromJsonAsync<ApiResponse.ApiResponse<CheckStockAvailabilityResponseModel>>();

        if (response is null || response.Data is null || !response.Data.IsAvailable)
            return false;

        return true;
    }
}