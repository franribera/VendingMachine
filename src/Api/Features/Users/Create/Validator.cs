using FluentValidation;

namespace Api.Features.Users.Create;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(r => r.Username).NotEmpty();
        RuleFor(r => r.Password).NotEmpty().MinimumLength(6);
        RuleFor(r => r.Role).NotEmpty();
    }
}