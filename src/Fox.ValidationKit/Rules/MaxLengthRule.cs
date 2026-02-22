//==================================================================================================
// Validation rule that checks if a string length does not exceed a maximum length.
// Treats null strings as valid (use NotNull/NotEmpty for null checks).
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a string length does not exceed a maximum length.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal sealed class MaxLengthRule<T>(string propertyName, int maxLength, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{

    //==============================================================================================
    /// <summary>
    /// Validates that the string length does not exceed the maximum length.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the length exceeds maximum, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        if (value == null)
        {
            return Success();
        }

        return value.Length <= maxLength ? Success() : CreateError(ValidationErrorCodes.MaxLength, $"{propertyName} must not exceed {maxLength} characters.", maxLength);
    }
}
