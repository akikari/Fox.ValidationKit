//==================================================================================================
// Validation rule for credit card number validation.
// Uses Luhn algorithm to validate credit card numbers.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a string is a valid credit card number (Luhn algorithm).
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal sealed class CreditCardRule<T>(string propertyName, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{
    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Validates that the string is a valid credit card number using Luhn algorithm.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the value is not a valid credit card number, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        var digitsOnly = new string([.. value.Where(char.IsDigit)]);

        if (digitsOnly.Length is < 13 or > 19)
        {
            return CreateError(ValidationErrorCodes.CreditCard, $"{propertyName} is not a valid credit card number.");
        }

        var isValid = IsValidLuhn(digitsOnly);

        return isValid ? Success() : CreateError(ValidationErrorCodes.CreditCard, $"{propertyName} is not a valid credit card number.");
    }

    #endregion

    #region Private Methods

    //==============================================================================================
    /// <summary>
    /// Validates a credit card number using the Luhn algorithm (modulus 10 checksum).
    /// </summary>
    /// <param name="number">The credit card number string containing only digits.</param>
    /// <returns>True if the number passes Luhn validation, otherwise false.</returns>
    //==============================================================================================
    private static bool IsValidLuhn(string number)
    {
        var sum = 0;
        var alternate = false;

        for (var i = number.Length - 1; i >= 0; i--)
        {
            var digit = int.Parse(number[i].ToString());

            if (alternate)
            {
                digit *= 2;

                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    #endregion
}
