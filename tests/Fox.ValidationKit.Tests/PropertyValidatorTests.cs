//==================================================================================================
// Unit tests for PropertyValidator internal class defensive code.
// Tests null parameter validation in constructors and methods.
//==================================================================================================

using FluentAssertions;
using Fox.ValidationKit.Rules;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for PropertyValidator defensive null checks.
/// </summary>
//==================================================================================================
public sealed class PropertyValidatorTests
{
    //==============================================================================================
    /// <summary>
    /// Test model for PropertyValidator tests.
    /// </summary>
    //==============================================================================================
    private sealed class TestModel
    {
        public int Value { get; set; }
    }

    //==============================================================================================
    /// <summary>
    /// Tests that PropertyValidator constructor throws for null propertySelector.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Constructor_should_throw_for_null_propertySelector()
    {
        var act = () => new PropertyValidator<TestModel, int>(null!, "Value");

        act.Should().Throw<ArgumentNullException>().WithParameterName("propertySelector");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that PropertyValidator constructor throws for null propertyName.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Constructor_should_throw_for_null_propertyName()
    {
        var act = () => new PropertyValidator<TestModel, int>(x => x.Value, null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("propertyName");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that AddRule throws for null rule.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void AddRule_should_throw_for_null_rule()
    {
        var validator = new PropertyValidator<TestModel, int>(x => x.Value, "Value");

        var act = () => validator.AddRule(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("rule");
    }
}

//==================================================================================================
/// <summary>
/// Tests for RuleBuilder defensive null checks.
/// </summary>
//==================================================================================================
public sealed class RuleBuilderTests
{
    //==============================================================================================
    /// <summary>
    /// Test model for RuleBuilder tests.
    /// </summary>
    //==============================================================================================
    private sealed class TestModel
    {
        public int Value { get; set; }
    }

    //==============================================================================================
    /// <summary>
    /// Tests that RuleBuilder constructor throws for null propertyValidator.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Constructor_should_throw_for_null_propertyValidator()
    {
        var act = () => new RuleBuilder<TestModel, int>(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("propertyValidator");
    }
}
