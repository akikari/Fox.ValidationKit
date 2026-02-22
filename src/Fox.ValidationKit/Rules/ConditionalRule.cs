//==================================================================================================
// Wrapper rule that applies validation conditionally based on a predicate.
// Supports When (execute if true) and Unless (execute if false) semantics.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validation rule wrapper that conditionally executes validation based on a predicate.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
internal sealed class ConditionalRule<T, TProperty>(IValidationRule<T, TProperty> innerRule, Func<T, bool> condition, bool executeWhenTrue) : IValidationRule<T, TProperty>
{
    #region Fields

    private readonly IValidationRule<T, TProperty> innerRule = innerRule ?? throw new ArgumentNullException(nameof(innerRule));
    private readonly Func<T, bool> condition = condition ?? throw new ArgumentNullException(nameof(condition));
    private readonly bool executeWhenTrue = executeWhenTrue;

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Validates the value if the condition is met.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <returns>Validation errors if condition is met and validation fails, otherwise empty.</returns>
    //==============================================================================================
    public IEnumerable<ValidationError> Validate(T instance, TProperty value)
    {
        var conditionResult = condition(instance);
        var shouldExecute = executeWhenTrue ? conditionResult : !conditionResult;

        if (!shouldExecute)
        {
            return [];
        }

        return innerRule.Validate(instance, value);
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates the value if the condition is met.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The property value to validate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task containing validation errors if condition is met and validation fails.</returns>
    //==============================================================================================
    public Task<IEnumerable<ValidationError>> ValidateAsync(T instance, TProperty value, CancellationToken cancellationToken = default)
    {
        var conditionResult = condition(instance);
        var shouldExecute = executeWhenTrue ? conditionResult : !conditionResult;

        if (!shouldExecute)
        {
            return Task.FromResult(Enumerable.Empty<ValidationError>());
        }

        return innerRule.ValidateAsync(instance, value, cancellationToken);
    }

    #endregion
}
