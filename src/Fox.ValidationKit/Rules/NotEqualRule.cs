//==================================================================================================
// Validation rule for inequality validation.
// Ensures a property value does not equal another property value.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a property value does not equal another property value.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
internal sealed class NotEqualRule<T, TProperty>(string propertyName, Func<T, TProperty> comparisonValue, string? errorMessage = null) : ValidationRuleBase<T, TProperty>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the value does not equal the comparison value.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>A validation error if values are equal, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, TProperty value)
    {
        var comparison = comparisonValue(instance);
        var areEqual = EqualityComparer<TProperty>.Default.Equals(value, comparison);

        return !areEqual ? Success() : CreateError(ValidationErrorCodes.NotEqual, $"{propertyName} must not be equal to the specified value.");
    }
}
