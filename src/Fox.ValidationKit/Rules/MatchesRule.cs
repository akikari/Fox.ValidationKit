//==================================================================================================
// Validation rule that checks if a string matches a regular expression pattern.
// Treats null strings as invalid (fails validation).
//==================================================================================================
using System.Text.RegularExpressions;

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a string matches a regular expression pattern.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal sealed class MatchesRule<T>(string propertyName, string pattern, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{
    private readonly Regex regex = new(pattern, RegexOptions.Compiled);

    //==============================================================================================
    /// <summary>
    /// Validates that the string value matches the regular expression pattern.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the value does not match the pattern, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        return regex.IsMatch(value) ? Success() : CreateError(ValidationErrorCodes.Matches, $"{propertyName} has an invalid format.");
    }
}
