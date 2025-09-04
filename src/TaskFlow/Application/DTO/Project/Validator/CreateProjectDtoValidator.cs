using FluentValidation;

namespace TaskFlow.Business.DTO.Project.Validator;
public class CreateProjectDtoValidator : AbstractValidator<ProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().MinimumLength(3);
        RuleFor(p => p.AdminId)
            .NotEmpty();
    }
}
