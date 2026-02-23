//==================================================================================================
// Abstract base class for creating strongly-typed validators with fluent rule configuration.
// Provides DSL for defining validation rules and executing validation logic.
//==================================================================================================
using System.Linq.Expressions;

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Abstract base class for creating validators with fluent validation rules.
/// </summary>
/// <typeparam name="T">The type of object to validate.</typeparam>
//==================================================================================================
public abstract class Validator<T>
{
    #region Fields

    private readonly List<IPropertyValidator<T>> propertyValidators = [];

    #endregion

    #region Properties

    //==============================================================================================
    /// <summary>
    /// Gets or sets the message provider for localized or custom error messages.
    /// </summary>
    //==============================================================================================
    internal IValidationMessageProvider? MessageProvider { get; private set; }

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Sets a custom message provider for localized or custom validation error messages.
    /// </summary>
    /// <param name="messageProvider">The message provider to use for error messages.</param>
    /// <returns>This validator instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when messageProvider is null.</exception>
    //==============================================================================================
    public Validator<T> UseMessageProvider(IValidationMessageProvider messageProvider)
    {
        MessageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));

        foreach (var validator in propertyValidators)
        {
            validator.SetMessageProvider(MessageProvider);
        }

        return this;
    }

    //==============================================================================================
    /// <summary>
    /// Validates the specified instance and returns the validation result.
    /// </summary>
    /// <param name="instance">The object instance to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> containing validation status and errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when instance is null.</exception>
    //==============================================================================================
    public ValidationResult Validate(T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        var result = ValidationResult.Success();

        foreach (var validator in propertyValidators)
        {
            var errors = validator.Validate(instance);

            foreach (var error in errors)
            {
                result.AddError(error);
            }
        }

        return result;
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates the specified instance and returns the validation result.
    /// </summary>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task containing a <see cref="ValidationResult"/> with validation status and errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when instance is null.</exception>
    //==============================================================================================
    public async Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken = default)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        var result = ValidationResult.Success();

        foreach (var validator in propertyValidators)
        {
            var errors = await validator.ValidateAsync(instance, cancellationToken);

            foreach (var error in errors)
            {
                result.AddError(error);
            }
        }

        return result;
    }

    #endregion

    #region Protected Methods

    //==============================================================================================
    /// <summary>
    /// Defines a validation rule for a specific property.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="propertySelector">Expression to select the property to validate.</param>
    /// <returns>A <see cref="RuleBuilder{T, TProperty}"/> for configuring validation rules.</returns>
    /// <exception cref="ArgumentNullException">Thrown when propertySelector is null.</exception>
    /// <exception cref="ArgumentException">Thrown when propertySelector is not a member expression.</exception>
    //==============================================================================================
    protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertySelector)
    {
        ArgumentNullException.ThrowIfNull(propertySelector);

        var propertyName = GetPropertyName(propertySelector);
        var compiledSelector = propertySelector.Compile();
        var propertyValidator = new PropertyValidator<T, TProperty>(compiledSelector, propertyName);

        if (MessageProvider != null)
        {
            propertyValidator.SetMessageProvider(MessageProvider);
        }

        propertyValidators.Add(propertyValidator);

        return new RuleBuilder<T, TProperty>(propertyValidator);
    }

    #endregion

    #region Private Methods

    //==============================================================================================
    /// <summary>
    /// Extracts the property name from a member expression.
    /// </summary>
    /// <param name="expression">The property selector expression.</param>
    /// <returns>The name of the property.</returns>
    /// <exception cref="ArgumentException">Thrown when expression is not a member expression.</exception>
    //==============================================================================================
    private static string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException("Expression must be a member expression", nameof(expression));
    }

    #endregion
}
