namespace TaskFlow.Business.DTO.Task.Validator;
using FluentValidation;
public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(task => task.Title)
            .NotNull().MinimumLength(3);
        RuleFor(task => task.ProjectId)
            .NotEmpty();
    }
}
