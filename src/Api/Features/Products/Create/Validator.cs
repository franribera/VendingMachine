using FluentValidation;

namespace Api.Features.Products.Create;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Price).GreaterThan(0);
        RuleFor(r => r.Quantity).GreaterThan(0);
    }
}