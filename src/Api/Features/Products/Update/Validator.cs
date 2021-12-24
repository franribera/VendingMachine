using FluentValidation;

namespace Api.Features.Products.Update;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(r => r.Price).GreaterThan(0);
        RuleFor(r => r.Quantity).GreaterThan(0);
    }
}