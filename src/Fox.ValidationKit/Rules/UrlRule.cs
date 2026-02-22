//==================================================================================================
// Validation rule for URL validation.
// Validates HTTP and HTTPS URLs using Uri.TryCreate.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a string is a valid URL.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal sealed class UrlRule<T>(string propertyName, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the string is a valid URL format.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the value is not a valid URL, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        var isValid = Uri.TryCreate(value, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        return isValid ? Success() : CreateError(ValidationErrorCodes.Url, $"{propertyName} is not a valid URL.");
    }
}
