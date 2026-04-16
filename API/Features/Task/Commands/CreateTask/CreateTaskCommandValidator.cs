using FluentValidation;

namespace API.Features.Task.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    private static readonly string[] ValidPriorities = { "Low", "Medium", "High" };

    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
           .NotEmpty().WithMessage("Title is required.")
           .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        
        RuleFor(x => x.Description)
           .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
           .When(x => x.Description is not null);

        RuleFor(x => x.Priority)
           .NotEmpty().WithMessage("Priority is required.")
           .Must(p => ValidPriorities.Contains(p, StringComparer.OrdinalIgnoreCase))
           .WithMessage("Priority must be Low, Medium, or High.");

        RuleFor(x => x.CategoryId)
           .GreaterThan(0).WithMessage("A valid CategoryId is required.");

        RuleFor(x => x.AssigneeEmail)
           .EmailAddress().WithMessage("AssigneeEmail must be a valid email address.")
           .When(x => !string.IsNullOrEmpty(x.AssigneeEmail));

        RuleFor(x => x.DueDate)
           .GreaterThan(DateTime.UtcNow).WithMessage("DueDate must be in the future.")
           .When(x => x.DueDate.HasValue);
    }
}
