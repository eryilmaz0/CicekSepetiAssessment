namespace BasketService.API.ApiResponse;

public class ApiResponse<TResponse>
{
    public TResponse Data { get; set; }

    public ApiResponse(TResponse data)
    {
        this.Data = data;
    }
}