//==================================================================================================
// Validation rule for enum validation.
// Ensures a value is a valid defined enum value.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a value is a valid enum value.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TEnum">The enum type being validated.</typeparam>
//==================================================================================================
internal sealed class IsInEnumRule<T, TEnum>(string propertyName, string? errorMessage = null) : ValidationRuleBase<T, TEnum>(propertyName, errorMessage) where TEnum : struct, Enum
{
    //==============================================================================================
    /// <summary>
    /// Validates that the value is a defined enum value.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The enum value to validate.</param>
    /// <returns>A validation error if the value is not defined in the enum, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, TEnum value)
    {
        var isValid = Enum.IsDefined<TEnum>(value);

        return isValid ? Success() : CreateError(ValidationErrorCodes.IsInEnum, $"{propertyName} is not a valid {typeof(TEnum).Name} value.", typeof(TEnum).Name);
    }
}
