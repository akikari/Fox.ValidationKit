//==================================================================================================
// Internal class that holds validation rules for a specific property.
// Executes all registered rules and collects validation errors.
//==================================================================================================
using System.Diagnostics.CodeAnalysis;
using Fox.ValidationKit.Rules;

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Manages validation rules for a specific property and executes validation.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
internal sealed class PropertyValidator<T, TProperty>(Func<T, TProperty> propertySelector, string propertyName) : IPropertyValidator<T>
{
    #region Fields

    private readonly List<IValidationRule<T, TProperty>> rules = [];
    private readonly Func<T, TProperty> propertySelector = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));
    private readonly string propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
    private CascadeMode cascadeMode = CascadeMode.Continue;
    private IValidationMessageProvider? messageProvider;

    #endregion

    #region Properties

    //==============================================================================================
    /// <summary>
    /// Gets the name of the property being validated.
    /// </summary>
    //==============================================================================================
    public string PropertyName => propertyName;

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Sets the cascade mode for this property validator.
    /// </summary>
    /// <param name="mode">The cascade mode to use.</param>
    //==============================================================================================
    public void SetCascadeMode(CascadeMode mode)
    {
        cascadeMode = mode;
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule to this property validator.
    /// </summary>
    /// <param name="rule">The validation rule to add.</param>
    //==============================================================================================
    public void AddRule(IValidationRule<T, TProperty> rule)
    {
        rules.Add(rule ?? throw new ArgumentNullException(nameof(rule)));

        if (rule is ValidationRuleBase<T, TProperty> ruleBase && messageProvider != null)
        {
            ruleBase.SetMessageProvider(messageProvider);
        }
    }

    //==============================================================================================
    /// <summary>
    /// Replaces the last added rule with a new rule (used for wrapping with conditional logic).
    /// </summary>
    /// <param name="newRule">The rule to replace the last rule with.</param>
    /// <remarks>
    /// The rules.Count == 0 check is defensive code - GetLastRule() is always called first
    /// by When/Unless methods, which throw before ReplaceLastRule is reached.
    /// </remarks>
    //==============================================================================================
    [ExcludeFromCodeCoverage]
    public void ReplaceLastRule(IValidationRule<T, TProperty> newRule)
    {
        if (rules.Count == 0)
        {
            throw new InvalidOperationException("No rules to replace.");
        }

        rules[^1] = newRule ?? throw new ArgumentNullException(nameof(newRule));
    }

    //==============================================================================================
    /// <summary>
    /// Gets the last added rule (used for wrapping with conditional logic).
    /// </summary>
    /// <returns>The last added rule, or null if no rules exist.</returns>
    //==============================================================================================
    public IValidationRule<T, TProperty>? GetLastRule()
    {
        return rules.Count > 0 ? rules[^1] : null;
    }

    //==============================================================================================
    /// <summary>
    /// Sets the message provider for all rules in this property validator.
    /// </summary>
    /// <param name="provider">The message provider to propagate to all rules.</param>
    //==============================================================================================
    public void SetMessageProvider(IValidationMessageProvider? provider)
    {
        messageProvider = provider;

        foreach (var rule in rules)
        {
            if (rule is ValidationRuleBase<T, TProperty> ruleBase)
            {
                ruleBase.SetMessageProvider(provider);
            }
        }
    }

    //==============================================================================================
    /// <summary>
    /// Validates the property value for the specified instance.
    /// </summary>
    /// <param name="instance">The object instance to validate.</param>
    /// <returns>A collection of validation errors.</returns>
    //==============================================================================================
    public IEnumerable<ValidationError> Validate(T instance)
    {
        var value = propertySelector(instance);
        var errors = new List<ValidationError>();

        foreach (var rule in rules)
        {
            var ruleErrors = rule.Validate(instance, value);
            errors.AddRange(ruleErrors);

            if (cascadeMode == CascadeMode.Stop && ruleErrors.Any())
            {
                break;
            }
        }

        return errors;
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates the property value for the specified instance.
    /// </summary>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task containing a collection of validation errors.</returns>
    //==============================================================================================
    public async Task<IEnumerable<ValidationError>> ValidateAsync(T instance, CancellationToken cancellationToken = default)
    {
        var value = propertySelector(instance);
        var errors = new List<ValidationError>();

        foreach (var rule in rules)
        {
            var ruleErrors = await rule.ValidateAsync(instance, value, cancellationToken);
            errors.AddRange(ruleErrors);

            if (cascadeMode == CascadeMode.Stop && ruleErrors.Any())
            {
                break;
            }
        }

        return errors;
    }

    #endregion
}
