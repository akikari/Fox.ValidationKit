//==================================================================================================
// Validation rule that executes a custom validation predicate function.
// Allows for arbitrary validation logic with support for both sync and async scenarios.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule that executes a custom validation predicate.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
internal sealed class CustomRule<T, TProperty> : ValidationRuleBase<T, TProperty>
{
    #region Fields

    private readonly Func<T, TProperty, bool> predicate;
    private readonly Func<T, TProperty, Task<bool>>? asyncPredicate;

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomRule{T, TProperty}"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property being validated.</param>
    /// <param name="predicate">The validation predicate (returns true if valid).</param>
    /// <param name="errorMessage">The error message to use when validation fails.</param>
    //==============================================================================================
    public CustomRule(string propertyName, Func<T, TProperty, bool> predicate, string errorMessage) : base(propertyName, errorMessage)
    {
        this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
    }

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomRule{T, TProperty}"/> class with async predicate.
    /// </summary>
    /// <param name="propertyName">The name of the property being validated.</param>
    /// <param name="asyncPredicate">The async validation predicate (returns true if valid).</param>
    /// <param name="errorMessage">The error message to use when validation fails.</param>
    //==============================================================================================
    public CustomRule(string propertyName, Func<T, TProperty, Task<bool>> asyncPredicate, string errorMessage) : base(propertyName, errorMessage)
    {
        this.asyncPredicate = asyncPredicate ?? throw new ArgumentNullException(nameof(asyncPredicate));
        predicate = (instance, value) => throw new NotSupportedException("Use ValidateAsync for async predicates.");
    }

    //==============================================================================================
    /// <summary>
    /// Validates the value using the custom predicate.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>A validation error if the predicate returns false, otherwise empty.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, TProperty value)
    {
        return predicate(instance, value) ? Success() : CreateError(ValidationErrorCodes.Must, errorMessage ?? $"{propertyName} is invalid.");
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates the value using the custom async predicate if available.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task containing validation errors.</returns>
    //==============================================================================================
    public override async Task<IEnumerable<ValidationError>> ValidateAsync(T instance, TProperty value, CancellationToken cancellationToken = default)
    {
        if (asyncPredicate != null)
        {
            var isValid = await asyncPredicate(instance, value);
            return isValid ? Success() : CreateError(ValidationErrorCodes.Must, errorMessage ?? $"{propertyName} is invalid.");
        }

        return await base.ValidateAsync(instance, value, cancellationToken);
    }

    #endregion
}
