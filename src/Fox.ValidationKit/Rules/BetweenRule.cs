//==================================================================================================
// Validation rule that checks if a comparable value is within a specified range.
// Uses IComparable for type-safe range validation.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a value is between a minimum and maximum value (inclusive).
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated (must be IComparable).</typeparam>
//==================================================================================================
internal sealed class BetweenRule<T, TProperty>(string propertyName, TProperty minimumValue, TProperty maximumValue, string? errorMessage = null) : ValidationRuleBase<T, TProperty>(propertyName, errorMessage)
    where TProperty : IComparable<TProperty>
{

    //==============================================================================================
    /// <summary>
    /// Validates that the value is between the minimum and maximum values (inclusive).
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>A validation error if the value is out of range, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, TProperty value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        if (value.CompareTo(minimumValue) >= 0 && value.CompareTo(maximumValue) <= 0)
        {
            return Success();
        }

        return CreateError(ValidationErrorCodes.Between, $"{propertyName} must be between {minimumValue} and {maximumValue}.", minimumValue, maximumValue);
    }
}
