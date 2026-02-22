//==================================================================================================
// Internal interface for property validators to enable polymorphic validation.
// Used by Validator to manage validation rules for different property types.
//==================================================================================================

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Internal interface for property validators to enable polymorphic validation.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
//==================================================================================================
internal interface IPropertyValidator<in T>
{
    //==============================================================================================
    /// <summary>
    /// Validates the property value for the specified instance.
    /// </summary>
    /// <param name="instance">The object instance to validate.</param>
    /// <returns>A collection of validation errors.</returns>
    //==============================================================================================
    IEnumerable<ValidationError> Validate(T instance);

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates the property value for the specified instance.
    /// </summary>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task containing a collection of validation errors.</returns>
    //==============================================================================================
    Task<IEnumerable<ValidationError>> ValidateAsync(T instance, CancellationToken cancellationToken);

    //==============================================================================================
    /// <summary>
    /// Sets the message provider for localized or custom error messages.
    /// </summary>
    /// <param name="provider">The message provider to propagate to all rules.</param>
    //==============================================================================================
    void SetMessageProvider(IValidationMessageProvider? provider);
}
