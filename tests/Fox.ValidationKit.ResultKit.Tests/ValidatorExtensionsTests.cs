//==================================================================================================
// Unit tests for Validator ResultKit extension methods.
// Tests ValidateAsResult and ValidateAsResultAsync methods.
//==================================================================================================

using FluentAssertions;
using Fox.ValidationKit;
using Fox.ValidationKit.ResultKit;

namespace Fox.ValidationKit.ResultKit.Tests;

//==================================================================================================
/// <summary>
/// Test model for ResultKit tests.
/// </summary>
//==================================================================================================
public sealed class User
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
}

//==================================================================================================
/// <summary>
/// Test validator for User.
/// </summary>
//==================================================================================================
public sealed class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().Matches(@"^[^@]+@[^@]+\.[^@]+$");
        RuleFor(x => x.Age).GreaterThan(0).LessThan(150);
    }
}

//==================================================================================================
/// <summary>
/// Tests for <see cref="ValidatorExtensions"/> class.
/// </summary>
//==================================================================================================
public sealed class ValidatorExtensionsTests
{
    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResult returns success Result for valid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsResult_should_return_success_for_valid_model()
    {
        var validator = new UserValidator();
        var user = new User { Name = "John", Email = "john@example.com", Age = 30 };

        var result = validator.ValidateAsResult(user);

        result.IsSuccess.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResult returns failure Result for invalid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsResult_should_return_failure_for_invalid_model()
    {
        var validator = new UserValidator();
        var user = new User { Name = "", Email = "invalid", Age = 0 };

        var result = validator.ValidateAsResult(user);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResult with value returns Result{T} with value for valid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsResult_with_value_should_return_success_result_with_value()
    {
        var validator = new UserValidator();
        var user = new User { Name = "John", Email = "john@example.com", Age = 30 };

        var result = validator.ValidateAsResultValue(user);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(user);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResult with value returns failure for invalid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsResult_with_value_should_return_failure_for_invalid_model()
    {
        var validator = new UserValidator();
        var user = new User { Name = "", Email = "invalid", Age = 0 };

        var result = validator.ValidateAsResultValue(user);

        result.IsSuccess.Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResult throws ArgumentNullException when validator is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsResult_should_throw_when_validator_is_null()
    {
        UserValidator validator = null!;
        var user = new User();

        var action = () => validator.ValidateAsResult(user);

        action.Should().Throw<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResultAsync returns success Result for valid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsResultAsync_should_return_success_for_valid_model()
    {
        var validator = new UserValidator();
        var user = new User { Name = "John", Email = "john@example.com", Age = 30 };

        var result = await validator.ValidateAsResultAsync(user);

        result.IsSuccess.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResultAsync returns failure Result for invalid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsResultAsync_should_return_failure_for_invalid_model()
    {
        var validator = new UserValidator();
        var user = new User { Name = "", Email = "invalid", Age = 0 };

        var result = await validator.ValidateAsResultAsync(user);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResultAsync with value returns Result{T} with value.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsResultAsync_with_value_should_return_success_with_value()
    {
        var validator = new UserValidator();
        var user = new User { Name = "John", Email = "john@example.com", Age = 30 };

        var result = await validator.ValidateAsResultValueAsync(user);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(user);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsResultAsync throws when validator is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsResultAsync_should_throw_when_validator_is_null()
    {
        UserValidator validator = null!;
        var user = new User();

        var action = async () => await validator.ValidateAsResultAsync(user);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that multiple validation errors are combined in Result error message.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsResult_should_combine_multiple_errors_in_message()
    {
        var validator = new UserValidator();
        var user = new User { Name = null, Email = null, Age = -1 };

        var result = validator.ValidateAsResult(user);

        result.Error.Should().Contain("Name");
        result.Error.Should().Contain("Email");
        result.Error.Should().Contain("Age");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsErrorsResult returns success for valid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsErrorsResult_should_return_success_for_valid_model()
    {
        var validator = new UserValidator();
        var user = new User { Name = "John", Email = "john@example.com", Age = 30 };

        var result = validator.ValidateAsErrorsResult(user);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsErrorsResult returns individual errors for invalid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsErrorsResult_should_return_individual_errors()
    {
        var validator = new UserValidator();
        var user = new User { Name = "", Email = "invalid", Age = -1 };

        var result = validator.ValidateAsErrorsResult(user);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsErrorsResult formats errors with error codes.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsErrorsResult_should_format_errors_with_error_codes()
    {
        var validator = new UserValidator();
        var user = new User { Name = "", Email = "invalid", Age = -1 };

        var result = validator.ValidateAsErrorsResult(user);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("FVK002"));
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsErrorsResult throws when validator is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidateAsErrorsResult_should_throw_when_validator_is_null()
    {
        UserValidator validator = null!;
        var user = new User();

        var action = () => validator.ValidateAsErrorsResult(user);

        action.Should().Throw<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsErrorsResultAsync returns success for valid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsErrorsResultAsync_should_return_success_for_valid_model()
    {
        var validator = new UserValidator();
        var user = new User { Name = "John", Email = "john@example.com", Age = 30 };

        var result = await validator.ValidateAsErrorsResultAsync(user);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsErrorsResultAsync returns individual errors for invalid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsErrorsResultAsync_should_return_individual_errors()
    {
        var validator = new UserValidator();
        var user = new User { Name = "", Email = "invalid", Age = -1 };

        var result = await validator.ValidateAsErrorsResultAsync(user);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsErrorsResultAsync throws when validator is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsErrorsResultAsync_should_throw_when_validator_is_null()
    {
        UserValidator validator = null!;
        var user = new User();

        var action = async () => await validator.ValidateAsErrorsResultAsync(user);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }
}
