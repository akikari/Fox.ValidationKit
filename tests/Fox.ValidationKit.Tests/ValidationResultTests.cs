//==================================================================================================
// Unit tests for ValidationResult class.
// Tests success/failure creation, error collection, and validation status.
//==================================================================================================

using FluentAssertions;
using Fox.ValidationKit;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for <see cref="ValidationResult"/> class.
/// </summary>
//==================================================================================================
public sealed class ValidationResultTests
{
    //==============================================================================================
    /// <summary>
    /// Tests that Success creates a valid ValidationResult with no errors.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Success_should_create_valid_result_with_no_errors()
    {
        var result = ValidationResult.Success();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Failure creates an invalid ValidationResult with errors.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Failure_should_create_invalid_result_with_errors()
    {
        var errors = new[]
        {
            new ValidationError("Email", "Email is required"),
            new ValidationError("Name", "Name is required")
        };

        var result = ValidationResult.Failure(errors);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Failure with single error creates result with one error.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Failure_should_create_result_with_single_error()
    {
        var result = ValidationResult.Failure("Email", "Email is required");

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].PropertyName.Should().Be("Email");
        result.Errors[0].Message.Should().Be("Email is required");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Failure throws ArgumentNullException when errors collection is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Failure_should_throw_when_errors_is_null()
    {
        var action = () => ValidationResult.Failure(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Failure with single error throws when propertyName is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Failure_should_throw_when_propertyName_is_null()
    {
        var action = () => ValidationResult.Failure(null!, "Message");

        action.Should().Throw<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Failure with single error throws when message is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Failure_should_throw_when_message_is_null()
    {
        var action = () => ValidationResult.Failure("Property", null!);

        action.Should().Throw<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Errors collection is read-only.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Errors_should_be_readonly()
    {
        var result = ValidationResult.Success();

        result.Errors.Should().BeAssignableTo<IReadOnlyList<ValidationError>>();
    }
}
