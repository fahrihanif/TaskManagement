using API.Features.Task.Commands.CreateTask;
using FluentValidation.TestHelper;

namespace Test.Validators;

public class CreateTaskCommandValidatorTests
{
    private readonly CreateTaskCommandValidator _validator = new();

    // Scenario 1: Empty title
    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateTaskCommand("", null, "High", null, null, 1);
        var result  = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Title)
              .WithErrorMessage("Title is required.");
    }

    // Scenario 2: Title too long
    [Fact]
    public void Validate_WhenTitleExceedsMaxLength_ShouldHaveValidationError()
    {
        var command = new CreateTaskCommand(
            new string('A', 201), null, "High", null, null, 1);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Title)
              .WithErrorMessage("Title must not exceed 200 characters.");
    }

    // Scenario 3: Invalid priority
    [Fact]
    public void Validate_WhenPriorityIsInvalid_ShouldHaveValidationError()
    {
        var command = new CreateTaskCommand(
            "Valid Title", null, "Critical", null, null, 1);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Priority)
              .WithErrorMessage("Priority must be Low, Medium, or High.");
    }

    // Scenario 4: CategoryId is zero
    [Fact]
    public void Validate_WhenCategoryIdIsZero_ShouldHaveValidationError()
    {
var command = new CreateTaskCommand(
            "Valid Title", null, "High", null, null, 0);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.CategoryId)
              .WithErrorMessage("A valid CategoryId is required.");
    }

    // Scenario 5: Invalid email format
    [Fact]
    public void Validate_WhenAssigneeEmailIsInvalid_ShouldHaveValidationError()
    {
        var command = new CreateTaskCommand(
            "Valid Title", null, "High", null, "not-an-email", 1);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.AssigneeEmail)
              .WithErrorMessage("AssigneeEmail must be a valid email address.");
    }

    // Scenario 6: Null email — optional field should not trigger error
    [Fact]
    public void Validate_WhenAssigneeEmailIsNull_ShouldNotHaveEmailValidationError()
    {
        var command = new CreateTaskCommand(
            "Valid Title", null, "High", null, null, 1);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.AssigneeEmail);
    }

    // Scenario 7: DueDate in the past
    [Fact]
    public void Validate_WhenDueDateIsInThePast_ShouldHaveValidationError()
    {
        var command = new CreateTaskCommand(
            "Valid Title", null, "High",
            DateTime.UtcNow.AddDays(-1), null, 1);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.DueDate)
              .WithErrorMessage("DueDate must be in the future.");
    }

    // Scenario 8: Null DueDate — optional field should not trigger error
    [Fact]
    public void Validate_WhenDueDateIsNull_ShouldNotHaveDueDateValidationError()
    {
        var command = new CreateTaskCommand(
            "Valid Title", null, "High", null, null, 1);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.DueDate);
    }

    // Scenario 9: Valid priorities including different casings
    [Theory]
    [InlineData("Low")]
    [InlineData("Medium")]
    [InlineData("High")]
    [InlineData("low")]
    [InlineData("HIGH")]
    public void Validate_WhenPriorityIsValid_ShouldNotHavePriorityError(
        string priority)
    {
        var command = new CreateTaskCommand(
            "Valid Title", null, priority, null, null, 1);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Priority);
    }

    // Scenario 10: All valid fields
    [Fact]
    public void Validate_WithAllValidFields_ShouldNotHaveAnyErrors()
    {
        var command = new CreateTaskCommand(
            Title:         "Valid Task",
            Description:   "A valid description",
            Priority:      "High",
            DueDate:       DateTime.UtcNow.AddDays(5),
            AssigneeEmail: "valid@email.com",
            CategoryId:    1
        );
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}