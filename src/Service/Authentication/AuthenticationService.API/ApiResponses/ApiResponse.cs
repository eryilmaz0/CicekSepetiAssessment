namespace AuthenticationService.API.ApiResponses;

public class ApiResponse<TResponse> where TResponse : class
{
    public TResponse Data { get; set; }

    public ApiResponse(TResponse data)
    {
        this.Data = data;
    }
}