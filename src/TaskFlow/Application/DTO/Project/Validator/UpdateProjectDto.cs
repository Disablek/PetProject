using FluentValidation;

namespace TaskFlow.Business.DTO.Project.Validator;
public class UpdateProjectDto : AbstractValidator<ProjectDto>
{
    public UpdateProjectDto()
    {
        RuleFor(p => p.AdminId)
            .NotEmpty();
        RuleFor(p => p.Name)
            .NotEmpty().MinimumLength(3);

    }
}
