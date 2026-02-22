//==================================================================================================
// Unit tests for common built-in validators.
// Tests cover EmailAddress, Url, CreditCard, Length, IsInEnum, and Must validators.
//==================================================================================================

using FluentAssertions;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for common built-in validators.
/// </summary>
//==================================================================================================
public sealed class CommonValidatorTests
{
    //==============================================================================================
    /// <summary>
    /// Test model for Email/URL/CreditCard validation.
    /// </summary>
    //==============================================================================================
    private sealed class ContactInfo
    {
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? CreditCard { get; set; }
    }

    private sealed class ContactInfoValidator : Validator<ContactInfo>
    {
        public ContactInfoValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Website).Url();
            RuleFor(x => x.CreditCard).CreditCard();
        }
    }

    [Fact]
    public void EmailAddress_should_pass_for_valid_email()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = "test@example.com", Website = "https://example.com", CreditCard = "4532015112830366" };

        var result = validator.Validate(contact);

        result.Errors.Should().NotContain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void EmailAddress_should_fail_for_invalid_email()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = "invalid-email", Website = "https://example.com", CreditCard = "4532015112830366" };

        var result = validator.Validate(contact);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void EmailAddress_should_fail_for_null()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = null, Website = "https://example.com", CreditCard = "4532015112830366" };

        var result = validator.Validate(contact);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Url_should_pass_for_valid_http_url()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = "test@example.com", Website = "http://example.com", CreditCard = "4532015112830366" };

        var result = validator.Validate(contact);

        result.Errors.Should().NotContain(e => e.PropertyName == "Website");
    }

    [Fact]
    public void Url_should_pass_for_valid_https_url()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = "test@example.com", Website = "https://www.example.com/path", CreditCard = "4532015112830366" };

        var result = validator.Validate(contact);

        result.Errors.Should().NotContain(e => e.PropertyName == "Website");
    }

    [Fact]
    public void Url_should_fail_for_invalid_url()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = "test@example.com", Website = "not-a-url", CreditCard = "4532015112830366" };

        var result = validator.Validate(contact);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Website");
    }

    [Fact]
    public void CreditCard_should_pass_for_valid_visa()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = "test@example.com", Website = "https://example.com", CreditCard = "4532015112830366" };

        var result = validator.Validate(contact);

        result.Errors.Should().NotContain(e => e.PropertyName == "CreditCard");
    }

    [Fact]
    public void CreditCard_should_pass_for_valid_mastercard()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = "test@example.com", Website = "https://example.com", CreditCard = "5425233430109903" };

        var result = validator.Validate(contact);

        result.Errors.Should().NotContain(e => e.PropertyName == "CreditCard");
    }

    [Fact]
    public void CreditCard_should_fail_for_invalid_card()
    {
        var validator = new ContactInfoValidator();
        var contact = new ContactInfo { Email = "test@example.com", Website = "https://example.com", CreditCard = "1234567890" };

        var result = validator.Validate(contact);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CreditCard");
    }

    //==============================================================================================
    /// <summary>
    /// Test model for Length validation.
    /// </summary>
    //==============================================================================================
    private sealed class UsernameData
    {
        public string? Username { get; set; }
    }

    private sealed class UsernameValidator : Validator<UsernameData>
    {
        public UsernameValidator()
        {
            RuleFor(x => x.Username).Length(3, 20);
        }
    }

    [Fact]
    public void Length_should_pass_when_within_range()
    {
        var validator = new UsernameValidator();
        var data = new UsernameData { Username = "johndoe" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Length_should_pass_at_minimum_boundary()
    {
        var validator = new UsernameValidator();
        var data = new UsernameData { Username = "abc" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Length_should_pass_at_maximum_boundary()
    {
        var validator = new UsernameValidator();
        var data = new UsernameData { Username = new string('a', 20) };

        var result = validator.Validate(data);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Length_should_fail_when_too_short()
    {
        var validator = new UsernameValidator();
        var data = new UsernameData { Username = "ab" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message.Contains("between 3 and 20"));
    }

    [Fact]
    public void Length_should_fail_when_too_long()
    {
        var validator = new UsernameValidator();
        var data = new UsernameData { Username = new string('a', 25) };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message.Contains("between 3 and 20"));
    }

    //==============================================================================================
    /// <summary>
    /// Test model for IsInEnum validation.
    /// </summary>
    //==============================================================================================
    private enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    private sealed class Task
    {
        public Priority TaskPriority { get; set; }
    }

    private sealed class TaskValidator : Validator<Task>
    {
        public TaskValidator()
        {
            RuleFor(x => x.TaskPriority).IsInEnum();
        }
    }

    [Fact]
    public void IsInEnum_should_pass_for_valid_enum_value()
    {
        var validator = new TaskValidator();
        var task = new Task { TaskPriority = Priority.High };

        var result = validator.Validate(task);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsInEnum_should_fail_for_invalid_enum_value()
    {
        var validator = new TaskValidator();
        var task = new Task { TaskPriority = (Priority)999 };

        var result = validator.Validate(task);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TaskPriority");
    }

    [Fact]
    public void IsInEnum_should_pass_for_all_defined_values()
    {
        var validator = new TaskValidator();

        foreach (Priority priority in Enum.GetValues<Priority>())
        {
            var task = new Task { TaskPriority = priority };
            var result = validator.Validate(task);
            result.IsValid.Should().BeTrue($"Priority.{priority} should be valid");
        }
    }

    //==============================================================================================
    /// <summary>
    /// Test model for Must (alias for Custom).
    /// </summary>
    //==============================================================================================
    private sealed class PasswordData
    {
        public string? Password { get; set; }
    }

    private sealed class PasswordMustValidator : Validator<PasswordData>
    {
        public PasswordMustValidator()
        {
            RuleFor(x => x.Password).Must((instance, password) => password?.Any(char.IsDigit) == true, "Password must contain at least one digit");
        }
    }

    [Fact]
    public void Must_should_pass_when_predicate_returns_true()
    {
        var validator = new PasswordMustValidator();
        var data = new PasswordData { Password = "pass123" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Must_should_fail_when_predicate_returns_false()
    {
        var validator = new PasswordMustValidator();
        var data = new PasswordData { Password = "password" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message.Contains("digit"));
    }

    [Fact]
    public void Must_should_use_custom_error_message()
    {
        var validator = new PasswordMustValidator();
        var data = new PasswordData { Password = "nodigits" };

        var result = validator.Validate(data);

        result.Errors.Should().Contain(e => e.Message == "Password must contain at least one digit");
    }
}
