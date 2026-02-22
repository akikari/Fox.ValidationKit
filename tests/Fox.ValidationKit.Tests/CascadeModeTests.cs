//==================================================================================================
// Unit tests for cascade mode validation behavior.
// Tests cover CascadeMode.Stop (fail-fast) and CascadeMode.Continue (collect all errors).
//==================================================================================================

using FluentAssertions;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for cascade mode validation behavior.
/// </summary>
//==================================================================================================
public sealed class CascadeModeTests
{
    //==============================================================================================
    /// <summary>
    /// Test model for cascade mode.
    /// </summary>
    //==============================================================================================
    private sealed class TestData
    {
        public string? Value { get; set; }
    }

    private sealed class CascadeStopValidator : Validator<TestData>
    {
        public CascadeStopValidator()
        {
            RuleFor(x => x.Value).Cascade(CascadeMode.Stop).NotEmpty().MinLength(5).MaxLength(10);
        }
    }

    private sealed class CascadeContinueValidator : Validator<TestData>
    {
        public CascadeContinueValidator()
        {
            RuleFor(x => x.Value).Cascade(CascadeMode.Continue).NotEmpty().MinLength(5).MaxLength(10);
        }
    }

    private sealed class DefaultCascadeValidator : Validator<TestData>
    {
        public DefaultCascadeValidator()
        {
            RuleFor(x => x.Value).NotEmpty().MinLength(5).MaxLength(10);
        }
    }

    [Fact]
    public void Cascade_stop_should_stop_after_first_failure()
    {
        var validator = new CascadeStopValidator();
        var data = new TestData { Value = "" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Cascade_stop_should_validate_all_when_passing()
    {
        var validator = new CascadeStopValidator();
        var data = new TestData { Value = "ValidValue" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Cascade_continue_should_collect_all_errors()
    {
        var validator = new CascadeContinueValidator();
        var data = new TestData { Value = "" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Cascade_continue_should_report_notEmpty_and_minLength_errors()
    {
        var validator = new CascadeContinueValidator();
        var data = new TestData { Value = "" };

        var result = validator.Validate(data);

        result.Errors.Should().Contain(e => e.Message.Contains("not be empty"));
        result.Errors.Should().Contain(e => e.Message.Contains("at least 5"));
    }

    [Fact]
    public void Default_cascade_mode_is_continue()
    {
        var validator = new DefaultCascadeValidator();
        var data = new TestData { Value = "" };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public async Task Cascade_stop_should_work_with_async_validation()
    {
        var validator = new CascadeStopValidator();
        var data = new TestData { Value = "" };

        var result = await validator.ValidateAsync(data);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }
}
