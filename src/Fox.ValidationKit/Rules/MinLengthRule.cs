//==================================================================================================
// Validation rule that checks if a string length meets a minimum length requirement.
// Treats null strings as invalid (fails validation).
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a string length meets a minimum length requirement.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal sealed class MinLengthRule<T>(string propertyName, int minLength, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the string length meets the minimum length requirement.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the length is less than minimum, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        return value.Length >= minLength ? Success() : CreateError(ValidationErrorCodes.MinLength, $"{propertyName} must be at least {minLength} characters.", minLength);
    }
}
