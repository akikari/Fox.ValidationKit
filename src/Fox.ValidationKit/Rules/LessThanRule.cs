//==================================================================================================
// Validation rule that checks if a comparable value is less than a specified maximum.
// Uses IComparable for type-safe comparison operations.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a value is less than a specified maximum.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated (must be IComparable).</typeparam>
//==================================================================================================
internal sealed class LessThanRule<T, TProperty>(string propertyName, TProperty maximumValue, string? errorMessage = null) : ValidationRuleBase<T, TProperty>(propertyName, errorMessage)
    where TProperty : IComparable<TProperty>
{

    //==============================================================================================
    /// <summary>
    /// Validates that the value is less than the maximum value.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>A validation error if the value is not less than maximum, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, TProperty value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        return value.CompareTo(maximumValue) < 0 ? Success() : CreateError(ValidationErrorCodes.LessThan, $"{propertyName} must be less than {maximumValue}.", maximumValue);
    }
}
