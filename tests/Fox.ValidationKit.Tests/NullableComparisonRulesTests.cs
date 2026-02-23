//==================================================================================================
// Unit tests for comparison rules with nullable reference types.
// Tests GreaterThan, LessThan, and Between with null string values.
//==================================================================================================

using FluentAssertions;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for comparison rules with nullable reference types.
/// </summary>
//==================================================================================================
public sealed class NullableComparisonRulesTests
{
    //==============================================================================================
    /// <summary>
    /// Test model with nullable string property.
    /// </summary>
    //==============================================================================================
    private sealed class StringModel
    {
        public string? Value { get; set; }
    }

    private sealed class StringGreaterThanValidator : Validator<StringModel>
    {
#pragma warning disable CS8631
        public StringGreaterThanValidator() => RuleFor(x => x.Value).GreaterThan("C");
#pragma warning restore CS8631
    }

    private sealed class StringLessThanValidator : Validator<StringModel>
    {
#pragma warning disable CS8631
        public StringLessThanValidator() => RuleFor(x => x.Value).LessThan("Z");
#pragma warning restore CS8631
    }

    private sealed class StringBetweenValidator : Validator<StringModel>
    {
#pragma warning disable CS8631
        public StringBetweenValidator() => RuleFor(x => x.Value).Between("A", "M");
#pragma warning restore CS8631
    }

    //==============================================================================================
    /// <summary>
    /// Tests that GreaterThan rule fails for null reference type value.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void GreaterThan_should_fail_for_null_reference_type()
    {
        var validator = new StringGreaterThanValidator();
        var model = new StringModel { Value = null };

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode == ValidationErrorCodes.NotNull);
        result.Errors.Should().Contain(e => e.Message.Contains("must not be null"));
    }

    //==============================================================================================
    /// <summary>
    /// Tests that LessThan rule fails for null reference type value.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void LessThan_should_fail_for_null_reference_type()
    {
        var validator = new StringLessThanValidator();
        var model = new StringModel { Value = null };

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode == ValidationErrorCodes.NotNull);
        result.Errors.Should().Contain(e => e.Message.Contains("must not be null"));
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Between rule fails for null reference type value.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Between_should_fail_for_null_reference_type()
    {
        var validator = new StringBetweenValidator();
        var model = new StringModel { Value = null };

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode == ValidationErrorCodes.NotNull);
        result.Errors.Should().Contain(e => e.Message.Contains("must not be null"));
    }

    //==============================================================================================
    /// <summary>
    /// Tests that GreaterThan passes for valid string value.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void GreaterThan_should_pass_for_valid_string_value()
    {
        var validator = new StringGreaterThanValidator();
        var model = new StringModel { Value = "D" };

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that LessThan passes for valid string value.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void LessThan_should_pass_for_valid_string_value()
    {
        var validator = new StringLessThanValidator();
        var model = new StringModel { Value = "A" };

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that Between passes for valid string value.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Between_should_pass_for_valid_string_value()
    {
        var validator = new StringBetweenValidator();
        var model = new StringModel { Value = "F" };

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }
}
