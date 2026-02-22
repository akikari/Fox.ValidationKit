//==================================================================================================
// Validator for User model demonstrating various validation rules.
// Shows usage of NotEmpty, MinLength, MaxLength, Matches, GreaterThan, LessThan, and Custom rules.
//==================================================================================================

namespace Fox.ValidationKit.Demo;

//==================================================================================================
/// <summary>
/// Validator for User model demonstrating various validation rules.
/// </summary>
//==================================================================================================
public sealed class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty("First name is required").MinLength(2).MaxLength(50);

        RuleFor(x => x.LastName).NotEmpty("Last name is required").MinLength(2).MaxLength(50);

        RuleFor(x => x.Email).NotEmpty("Email is required").Matches(@"^[^@]+@[^@]+\.[^@]+$", "Email format is invalid");

        RuleFor(x => x.Age).GreaterThan(0, "Age must be positive").LessThan(150, "Age must be realistic").Custom((user, age) => age >= 18, "User must be at least 18 years old");

        RuleFor(x => x.PhoneNumber).Matches(@"^\+?[\d\s\-\(\)]+$", "Phone number contains invalid characters");
    }
}
