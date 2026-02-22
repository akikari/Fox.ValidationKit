//==================================================================================================
// Abstract base class for validation rules providing common synchronous validation logic.
// Async validation delegates to synchronous validation by default.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Abstract base class for validation rules with default async behavior.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
internal abstract class ValidationRuleBase<T, TProperty>(string propertyName, string? errorMessage = null) : IValidationRule<T, TProperty>
{
    #region Fields

    protected readonly string propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
    protected readonly string? errorMessage = errorMessage;
    protected IValidationMessageProvider? messageProvider;

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Sets the message provider for localized error messages.
    /// </summary>
    /// <param name="provider">The message provider to use.</param>
    //==============================================================================================
    public void SetMessageProvider(IValidationMessageProvider? provider)
    {
        messageProvider = provider;
    }

    //==============================================================================================
    /// <summary>
    /// Validates the specified value and returns validation errors if any.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>A collection of validation errors, or empty if validation succeeds.</returns>
    //==============================================================================================
    public abstract IEnumerable<ValidationError> Validate(T instance, TProperty value);

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates the value by delegating to synchronous validation.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task containing validation errors.</returns>
    //==============================================================================================
    public virtual Task<IEnumerable<ValidationError>> ValidateAsync(T instance, TProperty value, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Validate(instance, value));
    }

    #endregion

    #region Protected Methods

    //==============================================================================================
    /// <summary>
    /// Creates a validation error with the specified or default message.
    /// </summary>
    /// <param name="defaultMessage">The default error message if no custom message was provided.</param>
    /// <returns>A collection containing a single validation error.</returns>
    //==============================================================================================
    protected IEnumerable<ValidationError> CreateError(string defaultMessage)
    {
        var message = errorMessage ?? defaultMessage;
        return [new ValidationError(propertyName, message)];
    }

    //==============================================================================================
    /// <summary>
    /// Creates a validation error with error code support for localization.
    /// </summary>
    /// <param name="errorCode">The error code identifying the validation failure.</param>
    /// <param name="defaultMessage">The default error message if no message provider is configured.</param>
    /// <param name="args">Optional arguments for message formatting.</param>
    /// <returns>A collection containing a single validation error.</returns>
    //==============================================================================================
    protected IEnumerable<ValidationError> CreateError(string errorCode, string defaultMessage, params object[] args)
    {
        var message = errorMessage ?? messageProvider?.GetMessage(errorCode, propertyName, args) ?? defaultMessage;
        return [new ValidationError(propertyName, message, errorCode)];
    }

    //==============================================================================================
    /// <summary>
    /// Returns an empty collection indicating validation success.
    /// </summary>
    /// <returns>An empty collection of validation errors.</returns>
    //==============================================================================================
    protected static IEnumerable<ValidationError> Success()
    {
        return [];
    }

    #endregion
}
