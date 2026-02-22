//==================================================================================================
// Unit tests for built-in validation rules.
// Tests NotNull, NotEmpty, GreaterThan, LessThan, Between, and string length rules.
//==================================================================================================

using FluentAssertions;
using Fox.ValidationKit;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for built-in validation rules.
/// </summary>
//==================================================================================================
public sealed class ValidationRulesTests
{
    //==============================================================================================
    /// <summary>
    /// Tests that NotNull rule fails for null values.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void NotNull_should_fail_for_null_value()
    {
        var validator = new NotNullValidator();
        var model = new TestModel { Name = null };

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].PropertyName.Should().Be("Name");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that NotNull rule succeeds for non-null values.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void NotNull_should_succeed_for_non_null_value()
    {
        var validator = new NotNullValidator();
        var model = new TestModel { Name = "John" };

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that NotEmpty rule fails for empty string.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void NotEmpty_should_fail_for_empty_string()
    {
        var validator = new NotEmptyValidator();
        var model = new TestModel { Name = "" };

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that NotEmpty rule fails for whitespace string.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void NotEmpty_should_fail_for_whitespace_string()
    {
        var validator = new NotEmptyValidator();
        var model = new TestModel { Name = "   " };

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that NotEmpty rule succeeds for non-empty string.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void NotEmpty_should_succeed_for_non_empty_string()
    {
        var validator = new NotEmptyValidator();
        var model = new TestModel { Name = "John" };

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that GreaterThan rule validates correctly.
    /// </summary>
    //==============================================================================================
    [Theory]
    [InlineData(0, false)]
    [InlineData(5, false)]
    [InlineData(10, false)]
    [InlineData(11, true)]
    [InlineData(100, true)]
    public void GreaterThan_should_validate_correctly(int age, bool expectedValid)
    {
        var validator = new GreaterThanValidator();
        var model = new TestModel { Age = age };

        var result = validator.Validate(model);

        result.IsValid.Should().Be(expectedValid);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that LessThan rule validates correctly.
    /// </summary>
    //==============================================================================================
    [Theory]
    [InlineData(0, true)]
    [InlineData(50, true)]
    [InlineData(99, true)]
    [InlineData(100, false)]
    [InlineData(150, false)]
    public void LessThan_should_validate_correctly(int age, bool expectedValid)
    {
        var validator = new LessThanValidator();
        var model = new TestModel { Age = age };

        var result = validator.Validate(model);

        result.IsValid.Should().Be(expectedValid);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Between rule validates correctly.
    /// </summary>
    //==============================================================================================
    [Theory]
    [InlineData(0, false)]
    [InlineData(18, true)]
    [InlineData(50, true)]
    [InlineData(65, true)]
    [InlineData(66, false)]
    public void Between_should_validate_correctly(int age, bool expectedValid)
    {
        var validator = new BetweenValidator();
        var model = new TestModel { Age = age };

        var result = validator.Validate(model);

        result.IsValid.Should().Be(expectedValid);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that MaxLength rule validates correctly.
    /// </summary>
    //==============================================================================================
    [Theory]
    [InlineData("", true)]
    [InlineData("John", true)]
    [InlineData("12345", true)]
    [InlineData("123456", false)]
    public void MaxLength_should_validate_correctly(string name, bool expectedValid)
    {
        var validator = new MaxLengthValidator();
        var model = new TestModel { Name = name };

        var result = validator.Validate(model);

        result.IsValid.Should().Be(expectedValid);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that MinLength rule validates correctly.
    /// </summary>
    //==============================================================================================
    [Theory]
    [InlineData("", false)]
    [InlineData("Jo", false)]
    [InlineData("John", true)]
    [InlineData("Johnny", true)]
    public void MinLength_should_validate_correctly(string name, bool expectedValid)
    {
        var validator = new MinLengthValidator();
        var model = new TestModel { Name = name };

        var result = validator.Validate(model);

        result.IsValid.Should().Be(expectedValid);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Matches rule validates email format correctly.
    /// </summary>
    //==============================================================================================
    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user.name@domain.co.uk", true)]
    [InlineData("invalid", false)]
    [InlineData("@example.com", false)]
    [InlineData("test@", false)]
    public void Matches_should_validate_email_format(string email, bool expectedValid)
    {
        var validator = new MatchesValidator();
        var model = new TestModel { Email = email };

        var result = validator.Validate(model);

        result.IsValid.Should().Be(expectedValid);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that custom error messages are used.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Custom_error_message_should_be_used()
    {
        var validator = new CustomMessageValidator();
        var model = new TestModel { Name = "" };

        var result = validator.Validate(model);

        result.Errors[0].Message.Should().Be("Please provide a name");
    }
}

//==================================================================================================
/// <summary>
/// Test validators for individual rules.
/// </summary>
//==================================================================================================
public sealed class NotNullValidator : Validator<TestModel>
{
    public NotNullValidator() => RuleFor(x => x.Name).NotNull();
}

public sealed class NotEmptyValidator : Validator<TestModel>
{
    public NotEmptyValidator() => RuleFor(x => x.Name).NotEmpty();
}

public sealed class GreaterThanValidator : Validator<TestModel>
{
    public GreaterThanValidator() => RuleFor(x => x.Age).GreaterThan(10);
}

public sealed class LessThanValidator : Validator<TestModel>
{
    public LessThanValidator() => RuleFor(x => x.Age).LessThan(100);
}

public sealed class BetweenValidator : Validator<TestModel>
{
    public BetweenValidator() => RuleFor(x => x.Age).Between(18, 65);
}

public sealed class MaxLengthValidator : Validator<TestModel>
{
    public MaxLengthValidator() => RuleFor(x => x.Name).MaxLength(5);
}

public sealed class MinLengthValidator : Validator<TestModel>
{
    public MinLengthValidator() => RuleFor(x => x.Name).MinLength(4);
}

public sealed class MatchesValidator : Validator<TestModel>
{
    public MatchesValidator() => RuleFor(x => x.Email).Matches(@"^[^@]+@[^@]+\.[^@]+$");
}

public sealed class CustomMessageValidator : Validator<TestModel>
{
    public CustomMessageValidator() => RuleFor(x => x.Name).NotEmpty("Please provide a name");
}
