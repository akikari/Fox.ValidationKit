//==================================================================================================
// Validation rule for collection not empty validation.
// Ensures a collection contains at least one element.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that ensures a collection is not empty.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TElement">The type of elements in the collection.</typeparam>
//==================================================================================================
internal sealed class CollectionNotEmptyRule<T, TElement>(string propertyName, string? errorMessage = null) : ValidationRuleBase<T, IEnumerable<TElement>?>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the collection is not null or empty.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The collection to validate.</param>
    /// <returns>A validation error if the collection is null or empty, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, IEnumerable<TElement>? value)
    {
        if (value == null || !value.Any())
        {
            return CreateError(ValidationErrorCodes.CollectionNotEmpty, $"{propertyName} must not be empty.");
        }

        return Success();
    }
}
