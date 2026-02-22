//==================================================================================================
// Extension methods for RuleBuilder providing fluent API for common validation rules.
// Enables method chaining for building complex validation scenarios.
//==================================================================================================
using Fox.ValidationKit.Rules;

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Extension methods for <see cref="RuleBuilder{T, TProperty}"/> to add validation rules.
/// </summary>
//==================================================================================================
public static class RuleBuilderExtensions
{
    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures the property value is not null.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> NotNull<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, string? errorMessage = null)
    {
        var rule = new NotNullRule<T, TProperty>(ruleBuilder.PropertyName, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a string is not null, empty, or whitespace.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, string?> NotEmpty<T>(this RuleBuilder<T, string?> ruleBuilder, string? errorMessage = null)
    {
        var rule = new NotEmptyRule<T>(ruleBuilder.PropertyName, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures the value is greater than a specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated (must be IComparable).</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="minimumValue">The minimum value (exclusive).</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> GreaterThan<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, TProperty minimumValue, string? errorMessage = null)
        where TProperty : IComparable<TProperty>
    {
        var rule = new GreaterThanRule<T, TProperty>(ruleBuilder.PropertyName, minimumValue, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures the value is less than a specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated (must be IComparable).</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="maximumValue">The maximum value (exclusive).</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> LessThan<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, TProperty maximumValue, string? errorMessage = null)
        where TProperty : IComparable<TProperty>
    {
        var rule = new LessThanRule<T, TProperty>(ruleBuilder.PropertyName, maximumValue, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures the value is between a minimum and maximum (inclusive).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated (must be IComparable).</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="minimumValue">The minimum value (inclusive).</param>
    /// <param name="maximumValue">The maximum value (inclusive).</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> Between<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, TProperty minimumValue, TProperty maximumValue, string? errorMessage = null)
        where TProperty : IComparable<TProperty>
    {
        var rule = new BetweenRule<T, TProperty>(ruleBuilder.PropertyName, minimumValue, maximumValue, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a string length does not exceed a maximum length.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="maxLength">The maximum allowed length.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, string?> MaxLength<T>(this RuleBuilder<T, string?> ruleBuilder, int maxLength, string? errorMessage = null)
    {
        var rule = new MaxLengthRule<T>(ruleBuilder.PropertyName, maxLength, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a string length meets a minimum length requirement.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="minLength">The minimum required length.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, string?> MinLength<T>(this RuleBuilder<T, string?> ruleBuilder, int minLength, string? errorMessage = null)
    {
        var rule = new MinLengthRule<T>(ruleBuilder.PropertyName, minLength, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a string matches a regular expression pattern.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, string?> Matches<T>(this RuleBuilder<T, string?> ruleBuilder, string pattern, string? errorMessage = null)
    {
        var rule = new MatchesRule<T>(ruleBuilder.PropertyName, pattern, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a custom validation rule with a predicate function.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="predicate">The validation predicate (returns true if valid).</param>
    /// <param name="errorMessage">The error message to use when validation fails.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> Custom<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, Func<T, TProperty, bool> predicate, string errorMessage)
    {
        var rule = new CustomRule<T, TProperty>(ruleBuilder.PropertyName, predicate, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a custom asynchronous validation rule with a predicate function.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="asyncPredicate">The async validation predicate (returns true if valid).</param>
    /// <param name="errorMessage">The error message to use when validation fails.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> CustomAsync<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, Func<T, TProperty, Task<bool>> asyncPredicate, string errorMessage)
    {
        var rule = new CustomRule<T, TProperty>(ruleBuilder.PropertyName, asyncPredicate, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a collection is not empty.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TElement">The type of elements in the collection.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, IEnumerable<TElement>?> NotEmpty<T, TElement>(this RuleBuilder<T, IEnumerable<TElement>?> ruleBuilder, string? errorMessage = null)
    {
        var rule = new CollectionNotEmptyRule<T, TElement>(ruleBuilder.PropertyName, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a collection has at least a minimum number of elements.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TElement">The type of elements in the collection.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="minCount">The minimum number of elements required.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, IEnumerable<TElement>?> MinCount<T, TElement>(this RuleBuilder<T, IEnumerable<TElement>?> ruleBuilder, int minCount, string? errorMessage = null)
    {
        var rule = new CollectionMinCountRule<T, TElement>(ruleBuilder.PropertyName, minCount, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a collection does not exceed a maximum number of elements.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TElement">The type of elements in the collection.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="maxCount">The maximum number of elements allowed.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, IEnumerable<TElement>?> MaxCount<T, TElement>(this RuleBuilder<T, IEnumerable<TElement>?> ruleBuilder, int maxCount, string? errorMessage = null)
    {
        var rule = new CollectionMaxCountRule<T, TElement>(ruleBuilder.PropertyName, maxCount, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that applies a predicate to each element in a collection.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TElement">The type of elements in the collection.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="predicate">The validation predicate for each element (returns true if valid).</param>
    /// <param name="errorMessage">The error message to use when an element fails validation.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, IEnumerable<TElement>?> ForEach<T, TElement>(this RuleBuilder<T, IEnumerable<TElement>?> ruleBuilder, Func<TElement, bool> predicate, string errorMessage)
    {
        var rule = new RuleForEachRule<T, TElement>(ruleBuilder.PropertyName, predicate, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a condition that determines when the preceding validation rule should execute.
    /// The rule executes only when the condition returns true.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="condition">The condition predicate (rule executes when this returns true).</param>
    /// <returns>The rule builder for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no rule exists to apply condition to.</exception>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> When<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, Func<T, bool> condition)
    {
        var propertyValidator = ruleBuilder.PropertyValidator;
        var lastRule = propertyValidator.GetLastRule() ?? throw new InvalidOperationException("When() must be called after a validation rule.");
        var conditionalRule = new ConditionalRule<T, TProperty>(lastRule, condition, executeWhenTrue: true);
        propertyValidator.ReplaceLastRule(conditionalRule);

        return ruleBuilder;
    }

    //==============================================================================================
    /// <summary>
    /// Adds a condition that determines when the preceding validation rule should NOT execute.
    /// The rule executes only when the condition returns false.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="condition">The condition predicate (rule executes when this returns false).</param>
    /// <returns>The rule builder for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no rule exists to apply condition to.</exception>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> Unless<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, Func<T, bool> condition)
    {
        var propertyValidator = ruleBuilder.PropertyValidator;
        var lastRule = propertyValidator.GetLastRule() ?? throw new InvalidOperationException("Unless() must be called after a validation rule.");
        var conditionalRule = new ConditionalRule<T, TProperty>(lastRule, condition, executeWhenTrue: false);
        propertyValidator.ReplaceLastRule(conditionalRule);

        return ruleBuilder;
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that delegates validation of a nested object to another validator.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the nested property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="validator">The validator to use for the nested object.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty?> SetValidator<T, TProperty>(this RuleBuilder<T, TProperty?> ruleBuilder, Validator<TProperty> validator)
    {
        var rule = new SetValidatorRule<T, TProperty>(ruleBuilder.PropertyName, validator);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures the property value equals another property value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="comparisonValue">Function to get the comparison value from the instance.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> Equal<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, Func<T, TProperty> comparisonValue, string? errorMessage = null)
    {
        var rule = new EqualRule<T, TProperty>(ruleBuilder.PropertyName, comparisonValue, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures the property value does not equal another property value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="comparisonValue">Function to get the comparison value from the instance.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> NotEqual<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, Func<T, TProperty> comparisonValue, string? errorMessage = null)
    {
        var rule = new NotEqualRule<T, TProperty>(ruleBuilder.PropertyName, comparisonValue, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a string is a valid email address.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, string?> EmailAddress<T>(this RuleBuilder<T, string?> ruleBuilder, string? errorMessage = null)
    {
        var rule = new EmailAddressRule<T>(ruleBuilder.PropertyName, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a string is a valid URL.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, string?> Url<T>(this RuleBuilder<T, string?> ruleBuilder, string? errorMessage = null)
    {
        var rule = new UrlRule<T>(ruleBuilder.PropertyName, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a string is a valid credit card number.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, string?> CreditCard<T>(this RuleBuilder<T, string?> ruleBuilder, string? errorMessage = null)
    {
        var rule = new CreditCardRule<T>(ruleBuilder.PropertyName, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a string length is within a specified range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="minLength">The minimum length (inclusive).</param>
    /// <param name="maxLength">The maximum length (inclusive).</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, string?> Length<T>(this RuleBuilder<T, string?> ruleBuilder, int minLength, int maxLength, string? errorMessage = null)
    {
        var rule = new LengthRule<T>(ruleBuilder.PropertyName, minLength, maxLength, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a validation rule that ensures a value is a valid enum value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="errorMessage">Optional custom error message.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TEnum> IsInEnum<T, TEnum>(this RuleBuilder<T, TEnum> ruleBuilder, string? errorMessage = null) where TEnum : struct, Enum
    {
        var rule = new IsInEnumRule<T, TEnum>(ruleBuilder.PropertyName, errorMessage);
        return ruleBuilder.AddRule(rule);
    }

    //==============================================================================================
    /// <summary>
    /// Adds a custom validation rule using a predicate (alias for Custom method).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="predicate">The validation predicate (returns true if valid).</param>
    /// <param name="errorMessage">The error message to use when validation fails.</param>
    /// <returns>The rule builder for method chaining.</returns>
    //==============================================================================================
    public static RuleBuilder<T, TProperty> Must<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder, Func<T, TProperty, bool> predicate, string errorMessage)
    {
        return ruleBuilder.Custom(predicate, errorMessage);
    }
}
