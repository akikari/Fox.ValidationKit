//==================================================================================================
// Tests for validation error message localization and custom message providers.
// Verifies IValidationMessageProvider integration with built-in validation rules.
//==================================================================================================

using FluentAssertions;
using Xunit;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for localization and custom message provider functionality.
/// </summary>
//==================================================================================================
public sealed class LocalizationTests
{
    private sealed class TestProduct
    {
        public string? Name { get; set; }
        public int Stock { get; set; }
        public string? Email { get; set; }
    }

    private sealed class TestProductValidator : Validator<TestProduct>
    {
        public TestProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Stock).GreaterThan(0);
            RuleFor(x => x.Email).EmailAddress();
        }
    }

    private sealed class HungarianMessageProvider : IValidationMessageProvider
    {
        public string GetMessage(string errorCode, string propertyName, params object[] args)
        {
            return errorCode switch
            {
                ValidationErrorCodes.NotEmpty => $"{propertyName} nem lehet üres.",
                ValidationErrorCodes.EmailAddress => $"{propertyName} nem érvényes email cím.",
                _ => $"{propertyName} érvénytelen."
            };
        }
    }

    //==============================================================================================
    /// <summary>
    /// Default validation without message provider should use English error messages.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validate_without_message_provider_should_use_default_english_messages()
    {
        var validator = new TestProductValidator();
        var product = new TestProduct { Name = "", Stock = 0, Email = "invalid" };

        var result = validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.Errors[0].Message.Should().Be("Name must not be empty.");
        result.Errors[0].ErrorCode.Should().Be(ValidationErrorCodes.NotEmpty);
    }

    //==============================================================================================
    /// <summary>
    /// Using message provider should return localized error messages.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validate_with_message_provider_should_use_localized_messages()
    {
        var validator = new TestProductValidator();
        validator.UseMessageProvider(new HungarianMessageProvider());

        var product = new TestProduct { Name = "", Stock = 0, Email = "invalid" };

        var result = validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.Errors[0].Message.Should().Be("Name nem lehet üres.");
        result.Errors[0].ErrorCode.Should().Be(ValidationErrorCodes.NotEmpty);
        result.Errors[2].Message.Should().Be("Email nem érvényes email cím.");
        result.Errors[2].ErrorCode.Should().Be(ValidationErrorCodes.EmailAddress);
    }

    private sealed class TestProductValidatorWithCustomMessage : Validator<TestProduct>
    {
        public TestProductValidatorWithCustomMessage()
        {
            RuleFor(x => x.Name).NotEmpty("Custom message");
        }
    }

    //==============================================================================================
    /// <summary>
    /// Custom error message should override message provider.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Custom_error_message_should_override_message_provider()
    {
        var validator = new TestProductValidatorWithCustomMessage();
        validator.UseMessageProvider(new HungarianMessageProvider());

        var product = new TestProduct { Name = "" };

        var result = validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Message.Should().Be("Custom message");
        result.Errors[0].ErrorCode.Should().Be(ValidationErrorCodes.NotEmpty);
    }

    private sealed class TestProductValidatorWithMultipleRules : Validator<TestProduct>
    {
        public TestProductValidatorWithMultipleRules()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).MinLength(5);
            RuleFor(x => x.Email).EmailAddress();
        }
    }

    //==============================================================================================
    /// <summary>
    /// Error codes should be properly set for all built-in validation rules.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Error_codes_should_be_set_for_built_in_rules()
    {
        var validator = new TestProductValidatorWithMultipleRules();

        var product = new TestProduct { Name = "A", Email = "invalid" };

        var result = validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors[0].ErrorCode.Should().Be(ValidationErrorCodes.MinLength);
        result.Errors[1].ErrorCode.Should().Be(ValidationErrorCodes.EmailAddress);
    }

    //==============================================================================================
    /// <summary>
    /// UseMessageProvider should support method chaining.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void UseMessageProvider_should_support_method_chaining()
    {
        var validator = new TestProductValidator();

        var result = validator.UseMessageProvider(new HungarianMessageProvider());

        result.Should().BeSameAs(validator);
    }

    //==============================================================================================
    /// <summary>
    /// UseMessageProvider should throw when provider is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void UseMessageProvider_should_throw_when_provider_is_null()
    {
        var validator = new TestProductValidator();

        var act = () => validator.UseMessageProvider(null!);

        act.Should().Throw<ArgumentNullException>();
    }
}
