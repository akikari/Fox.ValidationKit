//==================================================================================================
// Validation rule for email address validation.
// Uses GeneratedRegex for performance-optimized pattern matching.
//==================================================================================================
using System.Text.RegularExpressions;

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a string is a valid email address.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal sealed partial class EmailAddressRule<T>(string propertyName, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{
    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Validates that the string is a valid email address format.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the value is not a valid email address, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        return GetEmailRegex().IsMatch(value) ? Success() : CreateError(ValidationErrorCodes.EmailAddress, $"{propertyName} is not a valid email address.");
    }

    #endregion

    #region Private Methods

    //==============================================================================================
    /// <summary>
    /// Gets a compiled regex pattern for email address validation.
    /// </summary>
    /// <returns>A compiled Regex for validating email addresses.</returns>
    //==============================================================================================
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    private static partial Regex GetEmailRegex();

    #endregion
}
