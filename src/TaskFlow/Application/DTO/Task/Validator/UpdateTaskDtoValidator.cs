using FluentValidation;

namespace TaskFlow.Business.DTO.Task.Validator;
public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        RuleFor(t => t.Title)
            .NotNull().MinimumLength(3).WithMessage("Title is required."); 
        RuleFor(t => t.Status)
            .NotEmpty().WithMessage("Status is required.");
        RuleFor(t => t.Priority)
            .NotEmpty().WithMessage("Priority is required.");
    }
}
