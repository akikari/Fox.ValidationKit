//==================================================================================================
// Validation rule for string length range validation.
// Ensures string length is between minimum and maximum values (inclusive).
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a string length is within a specified range.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal sealed class LengthRule<T>(string propertyName, int minLength, int maxLength, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the string length is between min and max (inclusive).
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the length is out of range, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        var length = value.Length;

        if (length < minLength || length > maxLength)
        {
            return CreateError(ValidationErrorCodes.Length, $"{propertyName} must be between {minLength} and {maxLength} characters.", minLength, maxLength);
        }

        return Success();
    }
}
