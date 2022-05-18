using AuthenticationService.API.Models;
using FluentValidation;

namespace AuthenticationService.API.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email Field Can Not be Null or Empty.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password Field Can Not be Null or Empty.");
    }
}