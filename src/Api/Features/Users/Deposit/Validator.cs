using Api.Domain.ValueObjects;
using FluentValidation;

namespace Api.Features.Users.Deposit;

public class DepositRequestValidator : AbstractValidator<DepositRequest>
{
    public DepositRequestValidator()
    {
        RuleFor(r => r.Coin).GreaterThan(0);
        RuleFor(r => r.Coin).Must(Coin.IsAllowed).WithMessage("Invalid coin value.");
    }
}