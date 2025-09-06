using FluentValidation;

namespace TaskFlow.Business.DTO.User.Validator;
public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(user => user.PasswordHash)
            .NotNull().MinimumLength(5).WithMessage("Password length should be more than 5 symbols");
        RuleFor(user => user.Email)
            .NotNull();
        RuleFor(user => user.UserName)
            .NotNull().MinimumLength(3);

    }
}
