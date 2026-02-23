//==================================================================================================
// Unit tests for ValidationResult defensive code.
// Tests null parameter validation in ValidationResult constructor.
//==================================================================================================

using FluentAssertions;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for ValidationResult defensive null checks.
/// </summary>
//==================================================================================================
public sealed class ValidationResultDefensiveCodeTests
{
    //==============================================================================================
    /// <summary>
    /// Tests that ValidationResult private constructor throws for null errors collection.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Constructor_should_throw_for_null_errors()
    {
        var constructorInfo = typeof(ValidationResult).GetConstructor(
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
            null,
            [typeof(IEnumerable<ValidationError>)],
            null);

        var act = () => constructorInfo!.Invoke([null!]);

        act.Should().Throw<System.Reflection.TargetInvocationException>()
            .WithInnerException<ArgumentNullException>()
            .Which.ParamName.Should().Be("errors");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidationResult.AddError throws for null error.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void AddError_should_throw_for_null_error()
    {
        var result = ValidationResult.Success();
        var addErrorMethod = typeof(ValidationResult).GetMethod("AddError", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        var act = () => addErrorMethod!.Invoke(result, [null!]);

        act.Should().Throw<System.Reflection.TargetInvocationException>()
            .WithInnerException<ArgumentNullException>()
            .Which.ParamName.Should().Be("error");
    }
}
