//==================================================================================================
// Unit tests for ValidationError class.
// Tests record equality, property initialization, and null handling.
//==================================================================================================

using FluentAssertions;
using Fox.ValidationKit;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for <see cref="ValidationError"/> class.
/// </summary>
//==================================================================================================
public sealed class ValidationErrorTests
{
    //==============================================================================================
    /// <summary>
    /// Tests that ValidationError initializes correctly with valid parameters.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Constructor_should_initialize_properties_correctly()
    {
        var error = new ValidationError("Email", "Email is required");

        error.PropertyName.Should().Be("Email");
        error.Message.Should().Be("Email is required");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidationError throws ArgumentNullException when propertyName is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Constructor_should_throw_when_propertyName_is_null()
    {
        var action = () => new ValidationError(null!, "Message");

        action.Should().Throw<ArgumentNullException>().WithParameterName("propertyName");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that ValidationError throws ArgumentNullException when message is null.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Constructor_should_throw_when_message_is_null()
    {
        var action = () => new ValidationError("Property", null!);

        action.Should().Throw<ArgumentNullException>().WithParameterName("message");
    }

    //==============================================================================================
    /// <summary>
    /// Tests that two ValidationError instances with the same values are equal.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Equals_should_return_true_for_equal_errors()
    {
        var error1 = new ValidationError("Email", "Email is required");
        var error2 = new ValidationError("Email", "Email is required");

        error1.Should().Be(error2);
    }

    //==============================================================================================
    /// <summary>
    /// Tests that two ValidationError instances with different values are not equal.
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Equals_should_return_false_for_different_errors()
    {
        var error1 = new ValidationError("Email", "Email is required");
        var error2 = new ValidationError("Name", "Name is required");

        error1.Should().NotBe(error2);
    }
}
