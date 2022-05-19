using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.API.Entity;
using AuthenticationService.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.API.Services;

public class JwtTokenService : ITokenService
{
    private readonly JwtOptions _tokenOptions;

    public JwtTokenService(IOptions<JwtOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public async Task<CreateTokenResponse> CreateTokenAsync(CreateTokenRequest request)
    {
        var accessTokenExpiration = DateTime.Now.AddDays(_tokenOptions.TokenExpiration);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey)); 
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        
        var jwt = new JwtSecurityToken(issuer: _tokenOptions.Issuer, 
                                       audience: _tokenOptions.Audience, 
                                       expires: accessTokenExpiration,
                                       notBefore: DateTime.Now,
                                       claims: GetClaims(request.User),
                                       signingCredentials: signingCredentials);
          
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new() { IsSuccess = true, Token = token };
    }


    private IEnumerable<Claim> GetClaims(User user)
    {
        return new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Email, user.Email)
        };
    }
}