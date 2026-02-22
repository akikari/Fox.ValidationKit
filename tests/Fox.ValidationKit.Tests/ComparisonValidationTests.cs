//==================================================================================================
// Unit tests for comparison validation rules.
// Tests cover Equal and NotEqual validation for property comparison.
//==================================================================================================

using FluentAssertions;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for comparison validation (Equal/NotEqual).
/// </summary>
//==================================================================================================
public sealed class ComparisonValidationTests
{
    //==============================================================================================
    /// <summary>
    /// Test model for Equal/NotEqual validation.
    /// </summary>
    //==============================================================================================
    private sealed class PasswordChange
    {
        public string? Password { get; set; }
        public string? PasswordConfirmation { get; set; }
        public string? OldPassword { get; set; }
    }

    private sealed class PasswordChangeValidator : Validator<PasswordChange>
    {
        public PasswordChangeValidator()
        {
            RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password);
            RuleFor(x => x.Password).NotEqual(x => x.OldPassword);
        }
    }

    [Fact]
    public void Equal_should_pass_when_values_match()
    {
        var validator = new PasswordChangeValidator();
        var data = new PasswordChange { Password = "test123", PasswordConfirmation = "test123", OldPassword = "old123" };

        var result = validator.Validate(data);

        result.Errors.Should().NotContain(e => e.PropertyName == "PasswordConfirmation");
    }

    [Fact]
    public void Equal_should_fail_when_values_differ()
    {
        var validator = new PasswordChangeValidator();
        var data = new PasswordChange { Password = "test123", PasswordConfirmation = "different", OldPassword = "old123" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PasswordConfirmation");
    }

    [Fact]
    public void Equal_should_handle_null_values()
    {
        var validator = new PasswordChangeValidator();
        var data = new PasswordChange { Password = null, PasswordConfirmation = null, OldPassword = "old" };

        var result = validator.Validate(data);

        result.Errors.Should().NotContain(e => e.PropertyName == "PasswordConfirmation");
    }

    [Fact]
    public void NotEqual_should_pass_when_values_differ()
    {
        var validator = new PasswordChangeValidator();
        var data = new PasswordChange { Password = "new123", PasswordConfirmation = "new123", OldPassword = "old123" };

        var result = validator.Validate(data);

        result.Errors.Should().NotContain(e => e.PropertyName == "Password" && e.Message.Contains("not be equal"));
    }

    [Fact]
    public void NotEqual_should_fail_when_values_match()
    {
        var validator = new PasswordChangeValidator();
        var data = new PasswordChange { Password = "same123", PasswordConfirmation = "same123", OldPassword = "same123" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public void Complete_password_change_should_validate_correctly()
    {
        var validator = new PasswordChangeValidator();
        var validData = new PasswordChange { Password = "newPass123", PasswordConfirmation = "newPass123", OldPassword = "oldPass456" };

        var result = validator.Validate(validData);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Complete_password_change_should_fail_with_both_errors()
    {
        var validator = new PasswordChangeValidator();
        var invalidData = new PasswordChange { Password = "same", PasswordConfirmation = "different", OldPassword = "same" };

        var result = validator.Validate(invalidData);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }
}
