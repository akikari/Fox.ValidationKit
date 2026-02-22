//==================================================================================================
// Validation rule for nested object validation using a child validator.
// Delegates validation to another Validator and propagates errors with property path.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that delegates validation of a nested object to another validator.
/// </summary>
/// <typeparam name="T">The type of the parent object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the nested property being validated.</typeparam>
//==================================================================================================
internal sealed class SetValidatorRule<T, TProperty>(string propertyName, Validator<TProperty> childValidator) : IValidationRule<T, TProperty?>
{
    #region Fields

    private readonly string propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
    private readonly Validator<TProperty> childValidator = childValidator ?? throw new ArgumentNullException(nameof(childValidator));

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Validates the nested object using the child validator.
    /// </summary>
    /// <param name="instance">The parent object instance being validated.</param>
    /// <param name="value">The nested property value to validate.</param>
    /// <returns>Validation errors from the child validator with property path prefixed.</returns>
    //==============================================================================================
    public IEnumerable<ValidationError> Validate(T instance, TProperty? value)
    {
        if (value == null)
        {
            return [];
        }

        var result = childValidator.Validate(value);

        if (result.IsValid)
        {
            return [];
        }

        return result.Errors.Select(e => new ValidationError($"{propertyName}.{e.PropertyName}", e.Message));
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates the nested object using the child validator.
    /// </summary>
    /// <param name="instance">The parent object instance being validated.</param>
    /// <param name="value">The nested property value to validate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task containing validation errors with property path prefixed.</returns>
    //==============================================================================================
    public async Task<IEnumerable<ValidationError>> ValidateAsync(T instance, TProperty? value, CancellationToken cancellationToken = default)
    {
        if (value == null)
        {
            return [];
        }

        var result = await childValidator.ValidateAsync(value, cancellationToken);

        if (result.IsValid)
        {
            return [];
        }

        return result.Errors.Select(e => new ValidationError($"{propertyName}.{e.PropertyName}", e.Message));
    }

    #endregion
}
