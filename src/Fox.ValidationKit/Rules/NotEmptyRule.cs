//==================================================================================================
// Validation rule that checks if a string is not null, empty, or whitespace.
// Fails validation when the string value is null, empty, or contains only whitespace.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a string is not null, empty, or whitespace.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal sealed class NotEmptyRule<T>(string propertyName, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{

    //==============================================================================================
    /// <summary>
    /// Validates that the string value is not null, empty, or whitespace.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the value is empty, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? CreateError(ValidationErrorCodes.NotEmpty, $"{propertyName} must not be empty.") : Success();
    }
}
