//==================================================================================================
// Unit tests for ValidationResult to Result conversion extensions.
// Tests ToResult methods and error message formatting.
//==================================================================================================

using FluentAssertions;
using Fox.ValidationKit;
using Fox.ValidationKit.ResultKit;

namespace Fox.ValidationKit.ResultKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for <see cref="ValidationResultExtensions"/> class.
/// </summary>
//==================================================================================================
public sealed class ValidationResultExtensionsTests
{
    //==============================================================================================
    /// <summary>
    /// Tests that ToResult returns success Result for valid ValidationResult.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToResult_should_return_success_for_valid_validation_result()
    {
        var validationResult = ValidationResult.Success();

        var result = validationResult.ToResult();

        result.IsSuccess.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToResult returns failure Result for invalid ValidationResult.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToResult_should_return_failure_for_invalid_validation_result()
    {
        var validationResult = ValidationResult.Failure("Name", "Name is required");

        var result = validationResult.ToResult();

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Name");
        result.Error.Should().Contain("Name is required");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToResult combines multiple validation errors into single message.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToResult_should_combine_multiple_errors()
    {
        var errors = new[]
        {
            new ValidationError("Name", "Name is required"),
            new ValidationError("Email", "Email is invalid")
        };
        var validationResult = ValidationResult.Failure(errors);

        var result = validationResult.ToResult();

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Name: Name is required");
        result.Error.Should().Contain("Email: Email is invalid");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToResult throws ArgumentNullException when validationResult is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToResult_should_throw_when_validation_result_is_null()
    {
        ValidationResult validationResult = null!;

        var action = () => validationResult.ToResult();

        action.Should().Throw<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToResult with value returns success Result{T} for valid ValidationResult.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToResult_with_value_should_return_success_for_valid_validation_result()
    {
        var validationResult = ValidationResult.Success();
        var value = "test value";

        var result = validationResult.ToResult(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToResult with value returns failure Result{T} for invalid ValidationResult.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToResult_with_value_should_return_failure_for_invalid_validation_result()
    {
        var validationResult = ValidationResult.Failure("Property", "Property is invalid");
        var value = "test value";

        var result = validationResult.ToResult(value);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Property is invalid");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToResult with value throws when validationResult is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToResult_with_value_should_throw_when_validation_result_is_null()
    {
        ValidationResult validationResult = null!;

        var action = () => validationResult.ToResult("value");

        action.Should().Throw<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToErrorsResult returns success for valid ValidationResult.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToErrorsResult_should_return_success_for_valid_validation_result()
    {
        var validationResult = ValidationResult.Success();

        var result = validationResult.ToErrorsResult();

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToErrorsResult returns individual errors for invalid ValidationResult.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToErrorsResult_should_return_individual_errors()
    {
        var errors = new[]
        {
            new ValidationError("Name", "Name is required", ValidationErrorCodes.NotEmpty),
            new ValidationError("Email", "Email is invalid", ValidationErrorCodes.EmailAddress)
        };
        var validationResult = ValidationResult.Failure(errors);

        var result = validationResult.ToErrorsResult();

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors[0].Should().Contain("FVK002");
        result.Errors[0].Should().Contain("Name is required");
        result.Errors[1].Should().Contain("FVK301");
        result.Errors[1].Should().Contain("Email is invalid");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToErrorsResult formats errors with error code prefix when available.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToErrorsResult_should_format_errors_with_error_code_prefix()
    {
        var error = new ValidationError("Name", "Name is required", ValidationErrorCodes.NotEmpty);
        var validationResult = ValidationResult.Failure([error]);

        var result = validationResult.ToErrorsResult();

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Should().Be("FVK002: Name is required");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToErrorsResult falls back to property name when no error code is present.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToErrorsResult_should_fallback_to_property_name_without_error_code()
    {
        var error = new ValidationError("Name", "Name is required");
        var validationResult = ValidationResult.Failure([error]);

        var result = validationResult.ToErrorsResult();

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Should().Be("Name: Name is required");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ToErrorsResult throws ArgumentNullException when validationResult is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ToErrorsResult_should_throw_when_validation_result_is_null()
    {
        ValidationResult validationResult = null!;

        var action = () => validationResult.ToErrorsResult();

        action.Should().Throw<ArgumentNullException>();
    }
}
