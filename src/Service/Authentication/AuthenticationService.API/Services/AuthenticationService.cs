using AuthenticationService.API.Models;
using AuthenticationService.API.Repository;

namespace AuthenticationService.API.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = this._userRepository.GetUserByEmail(request.Email);

        if (user is null)
            return new() { IsSuccess = false, ResultMessage = "User Not Found." };

        if (!user.Password.Equals(request.Password))
        {
            return new() { IsSuccess = false, ResultMessage = "Password not Matching." };
        }

        var createTokenResult = await this._tokenService.CreateTokenAsync(new() { User = user });

        if (!createTokenResult.IsSuccess)
            return new() { IsSuccess = false, ResultMessage = "Token Not Created." };

        return new() { IsSuccess = true, ResultMessage = "Login Successfull.", Token = createTokenResult.Token };
    }
}