using FluentValidation;

namespace Api.Features.Users.Update;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(r => r.Password).NotEmpty().MinimumLength(6);
    }
}