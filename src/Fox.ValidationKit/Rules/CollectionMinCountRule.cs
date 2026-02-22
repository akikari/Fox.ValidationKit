//==================================================================================================
// Validation rule for collection minimum count validation.
// Ensures a collection has at least a specified number of elements.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a collection has at least a minimum number of elements.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TElement">The type of elements in the collection.</typeparam>
//==================================================================================================
internal sealed class CollectionMinCountRule<T, TElement>(string propertyName, int minCount, string? errorMessage = null) : ValidationRuleBase<T, IEnumerable<TElement>?>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the collection has at least the minimum number of elements.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The collection to validate.</param>
    /// <returns>A validation error if the count is less than minimum, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, IEnumerable<TElement>? value)
    {
        if (value == null)
        {
            return CreateError(ValidationErrorCodes.NotNull, $"{propertyName} must not be null.");
        }

        var count = value.Count();

        if (count < minCount)
        {
            return CreateError(ValidationErrorCodes.MinCount, $"{propertyName} must have at least {minCount} item(s).", minCount);
        }

        return Success();
    }
}
