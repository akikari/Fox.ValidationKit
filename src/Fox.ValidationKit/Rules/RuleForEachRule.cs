//==================================================================================================
// Validation rule for validating each element in a collection using a predicate.
// Applies a validation predicate to every element and collects all errors.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that applies a validation predicate to each element in a collection.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TElement">The type of elements in the collection.</typeparam>
//==================================================================================================
internal sealed class RuleForEachRule<T, TElement>(string propertyName, Func<TElement, bool> predicate, string errorMessage) : IValidationRule<T, IEnumerable<TElement>?>
{
    #region Fields

    private readonly string propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
    private readonly Func<TElement, bool> predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
    private readonly string errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Validates each element in the collection using the predicate.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The collection to validate.</param>
    /// <returns>A collection of validation errors from invalid elements.</returns>
    //==============================================================================================
    public IEnumerable<ValidationError> Validate(T instance, IEnumerable<TElement>? value)
    {
        if (value == null)
        {
            return [];
        }

        var errors = new List<ValidationError>();
        var index = 0;

        foreach (var element in value)
        {
            if (!predicate(element))
            {
                errors.Add(new ValidationError($"{propertyName}[{index}]", errorMessage));
            }

            index++;
        }

        return errors;
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates each element in the collection (delegates to synchronous version).
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The collection to validate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task containing validation errors from invalid elements.</returns>
    //==============================================================================================
    public Task<IEnumerable<ValidationError>> ValidateAsync(T instance, IEnumerable<TElement>? value, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Validate(instance, value));
    }

    #endregion
}

