//==================================================================================================
// Validation rule for collection maximum count validation.
// Ensures a collection does not exceed a specified number of elements.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a collection does not exceed a maximum number of elements.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TElement">The type of elements in the collection.</typeparam>
//==================================================================================================
internal sealed class CollectionMaxCountRule<T, TElement>(string propertyName, int maxCount, string? errorMessage = null) : ValidationRuleBase<T, IEnumerable<TElement>?>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the collection does not exceed the maximum number of elements.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The collection to validate.</param>
    /// <returns>A validation error if the count exceeds maximum, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, IEnumerable<TElement>? value)
    {
        if (value == null)
        {
            return Success();
        }

        var count = value.Count();

        if (count > maxCount)
        {
            return CreateError(ValidationErrorCodes.MaxCount, $"{propertyName} must not exceed {maxCount} item(s).", maxCount);
        }

        return Success();
    }
}
