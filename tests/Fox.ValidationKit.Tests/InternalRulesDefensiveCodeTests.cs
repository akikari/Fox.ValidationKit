//==================================================================================================
// Unit tests for internal rules defensive code.
// Tests null parameter validation in internal rule constructors.
//==================================================================================================

using FluentAssertions;
using Fox.ValidationKit.Rules;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for internal validation rules defensive null checks.
/// </summary>
//==================================================================================================
public sealed class InternalRulesDefensiveCodeTests
{
    //==============================================================================================
    /// <summary>
    /// Test model for rules tests.
    /// </summary>
    //==============================================================================================
    private sealed class TestModel
    {
        public string? Value { get; set; }
        public List<string>? Items { get; set; }
        public NestedModel? Nested { get; set; }
    }

    private sealed class NestedModel
    {
        public string? Name { get; set; }
    }

    private sealed class NestedValidator : Validator<NestedModel>
    {
        public NestedValidator() => RuleFor(x => x.Name).NotNull();
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ConditionalRule constructor throws for null innerRule.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ConditionalRule_should_throw_for_null_innerRule()
    {
        var act = () => new ConditionalRule<TestModel, string?>(null!, x => true, true);

        act.Should().Throw<ArgumentNullException>().WithParameterName("innerRule");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ConditionalRule constructor throws for null condition.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ConditionalRule_should_throw_for_null_condition()
    {
        var rule = new NotNullRule<TestModel, string?>("Value", null);
        var act = () => new ConditionalRule<TestModel, string?>(rule, null!, true);

        act.Should().Throw<ArgumentNullException>().WithParameterName("condition");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that CustomRule sync constructor throws for null predicate.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void CustomRule_should_throw_for_null_predicate()
    {
        Func<TestModel, string?, bool> nullPredicate = null!;
        var act = () => new CustomRule<TestModel, string?>("Value", nullPredicate, "Error");

        act.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that CustomRule async constructor throws for null asyncPredicate.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void CustomRule_should_throw_for_null_asyncPredicate()
    {
        var constructorInfo = typeof(CustomRule<TestModel, string?>).GetConstructor(
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
            null,
            [typeof(string), typeof(Func<TestModel, string?, Task<bool>>), typeof(string)],
            null);

        var act = () => constructorInfo!.Invoke(["Value", null!, "Error"]);

        act.Should().Throw<System.Reflection.TargetInvocationException>()
            .WithInnerException<ArgumentNullException>()
            .Which.ParamName.Should().Be("asyncPredicate");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that CustomRule uses default error message when errorMessage is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void CustomRule_should_use_default_error_message_when_null()
    {
        var rule = new CustomRule<TestModel, string?>("Value", (instance, value) => false, null!);
        var model = new TestModel { Value = "test" };

        var errors = rule.Validate(model, "test").ToList();

        errors.Should().ContainSingle();
        errors[0].Message.Should().Be("Value is invalid.");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that RuleForEachRule constructor throws for null propertyName.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void RuleForEachRule_should_throw_for_null_propertyName()
    {
        var act = () => new RuleForEachRule<TestModel, string>(null!, x => true, "Error");

        act.Should().Throw<ArgumentNullException>().WithParameterName("propertyName");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that RuleForEachRule constructor throws for null predicate.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void RuleForEachRule_should_throw_for_null_predicate()
    {
        var act = () => new RuleForEachRule<TestModel, string>("Items", null!, "Error");

        act.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that RuleForEachRule constructor throws for null errorMessage.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void RuleForEachRule_should_throw_for_null_errorMessage()
    {
        var act = () => new RuleForEachRule<TestModel, string>("Items", x => true, null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("errorMessage");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that SetValidatorRule constructor throws for null propertyName.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void SetValidatorRule_should_throw_for_null_propertyName()
    {
        var validator = new NestedValidator();
        var act = () => new SetValidatorRule<TestModel, NestedModel>(null!, validator);

        act.Should().Throw<ArgumentNullException>().WithParameterName("propertyName");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that SetValidatorRule constructor throws for null childValidator.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void SetValidatorRule_should_throw_for_null_childValidator()
    {
        var act = () => new SetValidatorRule<TestModel, NestedModel>("Nested", null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("childValidator");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidationRuleBase constructor throws for null propertyName.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void ValidationRuleBase_should_throw_for_null_propertyName()
    {
        var act = () => new NotNullRule<TestModel, string?>(null!, null);

        act.Should().Throw<ArgumentNullException>().WithParameterName("propertyName");
    }
}
