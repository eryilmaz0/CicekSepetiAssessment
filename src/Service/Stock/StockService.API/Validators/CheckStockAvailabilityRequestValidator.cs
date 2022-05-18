using FluentValidation;
using StockService.Entity.Dto;

namespace StockService.API.Validators;

public class CheckStockAvailabilityRequestValidator : AbstractValidator<CheckStockAvailabilityRequest>
{
    public CheckStockAvailabilityRequestValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id Field Can Not be Null or Empty.");
        RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0).WithMessage("Quantity Field Can Not be Null or Empty.");
    }
}