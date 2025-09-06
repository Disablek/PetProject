using FluentValidation;

namespace TaskFlow.Business.DTO.User.Validator;
public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(user => user.PasswordHash)
            .NotNull().MinimumLength(5);
        RuleFor(user => user.UserName)
            .NotNull().MinimumLength(5);
        RuleFor(user => user.FullName)
            .NotNull().MinimumLength(5);
    }
}
