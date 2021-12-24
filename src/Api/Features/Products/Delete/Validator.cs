using FluentValidation;

namespace Api.Features.Products.Delete;

public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(r => r.ProductId).GreaterThan(0);
    }
}