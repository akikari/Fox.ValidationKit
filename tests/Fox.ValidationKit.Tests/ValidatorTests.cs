//==================================================================================================
// Unit tests for Validator base class and rule configuration.
// Tests RuleFor DSL, validation execution, and async scenarios.
//==================================================================================================

using FluentAssertions;
using Fox.ValidationKit;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Test model for validation tests.
/// </summary>
//==================================================================================================
public sealed class TestModel
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }
}

//==================================================================================================
/// <summary>
/// Test validator for TestModel.
/// </summary>
//==================================================================================================
public sealed class TestModelValidator : Validator<TestModel>
{
    public TestModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Age).GreaterThan(0).LessThan(150);
        RuleFor(x => x.Email).NotEmpty().Matches(@"^[^@]+@[^@]+\.[^@]+$");
    }
}

//==================================================================================================
/// <summary>
/// Tests for <see cref="Validator{T}"/> class.
/// </summary>
//==================================================================================================
public sealed class ValidatorTests
{
    //==============================================================================================
    /// <summary>
    /// Tests that Validate returns success for valid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validate_should_return_success_for_valid_model()
    {
        var validator = new TestModelValidator();
        var model = new TestModel { Name = "John", Age = 30, Email = "john@example.com" };

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Validate returns failure with errors for invalid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validate_should_return_failure_for_invalid_model()
    {
        var validator = new TestModelValidator();
        var model = new TestModel { Name = "", Age = 0, Email = "invalid" };

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Validate collects all validation errors.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validate_should_collect_all_errors()
    {
        var validator = new TestModelValidator();
        var model = new TestModel { Name = null, Age = -1, Email = null };

        var result = validator.Validate(model);

        result.Errors.Should().Contain(e => e.PropertyName == "Name");
        result.Errors.Should().Contain(e => e.PropertyName == "Age");
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Validate throws ArgumentNullException when instance is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validate_should_throw_when_instance_is_null()
    {
        var validator = new TestModelValidator();

        var action = () => validator.Validate(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsync returns success for valid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsync_should_return_success_for_valid_model()
    {
        var validator = new TestModelValidator();
        var model = new TestModel { Name = "John", Age = 30, Email = "john@example.com" };

        var result = await validator.ValidateAsync(model);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsync returns failure for invalid model.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsync_should_return_failure_for_invalid_model()
    {
        var validator = new TestModelValidator();
        var model = new TestModel { Name = "", Age = 0, Email = "invalid" };

        var result = await validator.ValidateAsync(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidateAsync throws when instance is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task ValidateAsync_should_throw_when_instance_is_null()
    {
        var validator = new TestModelValidator();

        var action = async () => await validator.ValidateAsync(null!);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that custom validation rule works correctly.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Custom_rule_should_validate_correctly()
    {
        var validator = new CustomValidator();
        var model = new TestModel { Name = "John", Age = 17 };

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message.Contains("must be 18"));
    }
}

//==================================================================================================
/// <summary>
/// Validator with custom rule for testing.
/// </summary>
//==================================================================================================
public sealed class CustomValidator : Validator<TestModel>
{
    public CustomValidator()
    {
        RuleFor(x => x.Age).Custom((model, age) => age >= 18, "Age must be 18 or older");
    }
}
