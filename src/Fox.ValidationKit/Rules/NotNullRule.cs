//==================================================================================================
// Validation rule that checks if a value is not null.
// Fails validation when the value is null.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a value is not null.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
internal sealed class NotNullRule<T, TProperty>(string propertyName, string? errorMessage = null) : ValidationRuleBase<T, TProperty>(propertyName, errorMessage)
{

    //==============================================================================================
    /// <summary>
    /// Validates that the value is not null.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>A validation error if the value is null, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, TProperty value)
    {
        return value is null ? CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.") : Success();
    }
}
