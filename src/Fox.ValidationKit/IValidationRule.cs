//==================================================================================================
// Interface for validation rules that can validate a value and return validation errors.
// Supports both synchronous and asynchronous validation scenarios.
//==================================================================================================

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Defines a validation rule that can validate a value and return validation errors.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
public interface IValidationRule<T, TProperty>
{
    //==============================================================================================
    /// <summary>
    /// Validates the specified value and returns validation errors if any.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>A collection of validation errors, or an empty collection if validation succeeds.</returns>
    //==============================================================================================
    IEnumerable<ValidationError> Validate(T instance, TProperty value);

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates the specified value and returns validation errors if any.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing validation errors.</returns>
    //==============================================================================================
    Task<IEnumerable<ValidationError>> ValidateAsync(T instance, TProperty value, CancellationToken cancellationToken = default);
}
