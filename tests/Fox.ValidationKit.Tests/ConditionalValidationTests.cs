//==================================================================================================
// Unit tests for conditional validation features.
// Tests cover When and Unless validation rule conditions.
//==================================================================================================

using FluentAssertions;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for conditional validation (When/Unless).
/// </summary>
//==================================================================================================
public sealed class ConditionalValidationTests
{
    //==============================================================================================
    /// <summary>
    /// Test model for conditional validation.
    /// </summary>
    //==============================================================================================
    private sealed class Account
    {
        public bool IsCompany { get; set; }
        public string? CompanyName { get; set; }
        public string? PersonalName { get; set; }
    }

    //==============================================================================================
    /// <summary>
    /// Validator with When/Unless conditions.
    /// </summary>
    //==============================================================================================
    private sealed class AccountValidator : Validator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().When(x => x.IsCompany);
            RuleFor(x => x.PersonalName).NotEmpty().Unless(x => x.IsCompany);
        }
    }

    [Fact]
    public void When_should_execute_rule_when_condition_is_true()
    {
        var validator = new AccountValidator();
        var account = new Account { IsCompany = true, CompanyName = "" };

        var result = validator.Validate(account);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CompanyName");
    }

    [Fact]
    public void When_should_skip_rule_when_condition_is_false()
    {
        var validator = new AccountValidator();
        var account = new Account { IsCompany = false, CompanyName = "" };

        var result = validator.Validate(account);

        result.Errors.Should().NotContain(e => e.PropertyName == "CompanyName");
    }

    [Fact]
    public void When_should_pass_when_condition_true_and_value_valid()
    {
        var validator = new AccountValidator();
        var account = new Account { IsCompany = true, CompanyName = "ACME Corp" };

        var result = validator.Validate(account);

        result.Errors.Should().NotContain(e => e.PropertyName == "CompanyName");
    }

    [Fact]
    public void Unless_should_execute_rule_when_condition_is_false()
    {
        var validator = new AccountValidator();
        var account = new Account { IsCompany = false, PersonalName = "" };

        var result = validator.Validate(account);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PersonalName");
    }

    [Fact]
    public void Unless_should_skip_rule_when_condition_is_true()
    {
        var validator = new AccountValidator();
        var account = new Account { IsCompany = true, PersonalName = "" };

        var result = validator.Validate(account);

        result.Errors.Should().NotContain(e => e.PropertyName == "PersonalName");
    }

    [Fact]
    public void Unless_should_pass_when_condition_false_and_value_valid()
    {
        var validator = new AccountValidator();
        var account = new Account { IsCompany = false, PersonalName = "John Doe" };

        var result = validator.Validate(account);

        result.Errors.Should().NotContain(e => e.PropertyName == "PersonalName");
    }

    [Fact]
    public void Multiple_conditional_rules_should_work_together()
    {
        var validator = new AccountValidator();
        var companyAccount = new Account { IsCompany = true, CompanyName = "ACME", PersonalName = "Ignored" };
        var personalAccount = new Account { IsCompany = false, CompanyName = "Ignored", PersonalName = "John" };

        var companyResult = validator.Validate(companyAccount);
        var personalResult = validator.Validate(personalAccount);

        companyResult.IsValid.Should().BeTrue();
        personalResult.IsValid.Should().BeTrue();
    }

    private sealed class AsyncAccountValidator : Validator<Account>
    {
        public AsyncAccountValidator()
        {
            RuleFor(x => x.CompanyName).CustomAsync(async (instance, value) =>
            {
                await Task.Delay(1);
                return !string.IsNullOrEmpty(value);
            }, "Company name is required").When(x => x.IsCompany);

            RuleFor(x => x.PersonalName).CustomAsync(async (instance, value) =>
            {
                await Task.Delay(1);
                return !string.IsNullOrEmpty(value);
            }, "Personal name is required").Unless(x => x.IsCompany);
        }
    }

    [Fact]
    public async Task When_should_execute_async_rule_when_condition_is_true()
    {
        var validator = new AsyncAccountValidator();
        var account = new Account { IsCompany = true, CompanyName = "" };

        var result = await validator.ValidateAsync(account);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CompanyName");
    }

    [Fact]
    public async Task When_should_skip_async_rule_when_condition_is_false()
    {
        var validator = new AsyncAccountValidator();
        var account = new Account { IsCompany = false, CompanyName = "" };

        var result = await validator.ValidateAsync(account);

        result.Errors.Should().NotContain(e => e.PropertyName == "CompanyName");
    }

    [Fact]
    public async Task Unless_should_execute_async_rule_when_condition_is_false()
    {
        var validator = new AsyncAccountValidator();
        var account = new Account { IsCompany = false, PersonalName = "" };

        var result = await validator.ValidateAsync(account);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PersonalName");
    }

    [Fact]
    public async Task Unless_should_skip_async_rule_when_condition_is_true()
    {
        var validator = new AsyncAccountValidator();
        var account = new Account { IsCompany = true, PersonalName = "" };

        var result = await validator.ValidateAsync(account);

        result.Errors.Should().NotContain(e => e.PropertyName == "PersonalName");
    }

    private sealed class EmptyValidator : Validator<Account>
    {
        public EmptyValidator()
        {
        }

        public void CallWhenWithoutRule()
        {
            RuleFor(x => x.CompanyName).When(x => x.IsCompany);
        }

        public void CallUnlessWithoutRule()
        {
            RuleFor(x => x.CompanyName).Unless(x => x.IsCompany);
        }
    }

    //==============================================================================================
    /// <summary>
    /// Tests that When throws when called without a preceding rule.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void When_should_throw_when_called_without_rule()
    {
        var validator = new EmptyValidator();

        var act = () => validator.CallWhenWithoutRule();

        act.Should().Throw<InvalidOperationException>().WithMessage("*must be called after*");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Unless throws when called without a preceding rule.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Unless_should_throw_when_called_without_rule()
    {
        var validator = new EmptyValidator();

        var act = () => validator.CallUnlessWithoutRule();

        act.Should().Throw<InvalidOperationException>().WithMessage("*must be called after*");
    }
}
