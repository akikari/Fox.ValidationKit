//==================================================================================================
// Fluent API builder for configuring validation rules on a specific property.
// Provides extension methods for common validation scenarios.
//==================================================================================================

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Fluent builder for configuring validation rules on a specific property.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TProperty">The type of the property being validated.</typeparam>
//==================================================================================================
public sealed class RuleBuilder<T, TProperty>
{
    #region Fields

    private readonly PropertyValidator<T, TProperty> propertyValidator;

    #endregion

    #region Constructors

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="RuleBuilder{T, TProperty}"/> class.
    /// </summary>
    /// <param name="propertyValidator">The property validator to configure.</param>
    //==============================================================================================
    internal RuleBuilder(PropertyValidator<T, TProperty> propertyValidator)
    {
        this.propertyValidator = propertyValidator ?? throw new ArgumentNullException(nameof(propertyValidator));
    }

    #endregion

    #region Properties

    //==============================================================================================
    /// <summary>
    /// Gets the name of the property being validated.
    /// </summary>
    //==============================================================================================
    internal string PropertyName => propertyValidator.PropertyName;

    //==============================================================================================
    /// <summary>
    /// Gets the property validator (internal use only for extensions).
    /// </summary>
    //==============================================================================================
    internal PropertyValidator<T, TProperty> PropertyValidator => propertyValidator;

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Adds a custom validation rule.
    /// </summary>
    /// <param name="rule">The validation rule to add.</param>
    /// <returns>This builder for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when rule is null.</exception>
    //==============================================================================================
    public RuleBuilder<T, TProperty> AddRule(IValidationRule<T, TProperty> rule)
    {
        propertyValidator.AddRule(rule);
        return this;
    }

    //==============================================================================================
    /// <summary>
    /// Sets the cascade mode for this property's validation rules.
    /// </summary>
    /// <param name="cascadeMode">The cascade mode to use.</param>
    /// <returns>This builder for method chaining.</returns>
    //==============================================================================================
    public RuleBuilder<T, TProperty> Cascade(CascadeMode cascadeMode)
    {
        propertyValidator.SetCascadeMode(cascadeMode);
        return this;
    }

    #endregion
}
