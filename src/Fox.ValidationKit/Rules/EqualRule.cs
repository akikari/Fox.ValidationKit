//==================================================================================================
// Validation rule for equality validation.
// Ensures a property value equals another property value.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a property value equals another property value.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
internal sealed class EqualRule<T, TProperty>(string propertyName, Func<T, TProperty> comparisonValue, string? errorMessage = null) : ValidationRuleBase<T, TProperty>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the value equals the comparison value.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>A validation error if values are not equal, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, TProperty value)
    {
        var comparison = comparisonValue(instance);
        var areEqual = EqualityComparer<TProperty>.Default.Equals(value, comparison);

        return areEqual ? Success() : CreateError(ValidationErrorCodes.Equal, $"{propertyName} must be equal to the specified value.");
    }
}
