using FluentValidation;

namespace Api.Features.Products.Purchase;

public class PurchaseProductRequestValidator : AbstractValidator<PurchaseProductRequest>
{
    public PurchaseProductRequestValidator()
    {
        RuleFor(r => r.ProductId).GreaterThan(0);
        RuleFor(r => r.Quantity).GreaterThan(0);
    }
}