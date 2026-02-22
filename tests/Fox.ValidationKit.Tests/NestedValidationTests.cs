//==================================================================================================
// Unit tests for nested object validation using SetValidator.
// Tests cover validation of nested objects and property path composition.
//==================================================================================================

using FluentAssertions;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for nested object validation with SetValidator.
/// </summary>
//==================================================================================================
public sealed class NestedValidationTests
{
    //==============================================================================================
    /// <summary>
    /// Test models for nested validation.
    /// </summary>
    //==============================================================================================
    private sealed class Address
    {
        public string? City { get; set; }
        public string? ZipCode { get; set; }
    }

    private sealed class Person
    {
        public string? Name { get; set; }
        public Address? HomeAddress { get; set; }
    }

    private sealed class AddressValidator : Validator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty().Length(5, 10);
        }
    }

    private sealed class PersonValidator : Validator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.HomeAddress).SetValidator(new AddressValidator());
        }
    }

    [Fact]
    public void SetValidator_should_validate_nested_object()
    {
        var validator = new PersonValidator();
        var person = new Person { Name = "John", HomeAddress = new Address { City = "", ZipCode = "" } };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "HomeAddress.City");
        result.Errors.Should().Contain(e => e.PropertyName == "HomeAddress.ZipCode");
    }

    [Fact]
    public void SetValidator_should_pass_when_nested_object_is_valid()
    {
        var validator = new PersonValidator();
        var person = new Person { Name = "John", HomeAddress = new Address { City = "New York", ZipCode = "12345" } };

        var result = validator.Validate(person);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SetValidator_should_skip_when_nested_object_is_null()
    {
        var validator = new PersonValidator();
        var person = new Person { Name = "John", HomeAddress = null };

        var result = validator.Validate(person);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SetValidator_should_compose_property_paths_correctly()
    {
        var validator = new PersonValidator();
        var person = new Person { Name = "John", HomeAddress = new Address { City = "Valid", ZipCode = "X" } };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "HomeAddress.ZipCode");
    }

    [Fact]
    public async Task SetValidator_should_work_with_async_validation()
    {
        var validator = new PersonValidator();
        var person = new Person { Name = "John", HomeAddress = new Address { City = "", ZipCode = "" } };

        var result = await validator.ValidateAsync(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "HomeAddress.City");
    }
}
