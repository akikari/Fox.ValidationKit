//==================================================================================================
// Extension methods for Validator providing direct Result-based validation.
// Enables fluent API for validating objects and returning ResultKit Results.
//==================================================================================================
using Fox.ResultKit;

namespace Fox.ValidationKit.ResultKit;

//==================================================================================================
/// <summary>
/// Extension methods for <see cref="Validator{T}"/> to perform Result-based validation.
/// </summary>
//==================================================================================================
public static class ValidatorExtensions
{
    //==============================================================================================
    /// <summary>
    /// Validates an instance and returns a <see cref="Result"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="validator">The validator instance.</param>
    /// <param name="instance">The object instance to validate.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure with validation errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validator or instance is null.</exception>
    //==============================================================================================
    public static Result ValidateAsResult<T>(this Validator<T> validator, T instance)
    {
        ArgumentNullException.ThrowIfNull(validator);

        var validationResult = validator.Validate(instance);
        return validationResult.ToResult();
    }

    //==============================================================================================
    /// <summary>
    /// Validates an instance and returns a <see cref="Result{T}"/> with the validated value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="validator">The validator instance.</param>
    /// <param name="instance">The object instance to validate.</param>
    /// <returns>A <see cref="Result{T}"/> with the value if validation succeeded, otherwise failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validator or instance is null.</exception>
    //==============================================================================================
    public static Result<T> ValidateAsResultValue<T>(this Validator<T> validator, T instance)
    {
        ArgumentNullException.ThrowIfNull(validator);

        var validationResult = validator.Validate(instance);
        return validationResult.ToResult(instance);
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates an instance and returns a <see cref="Result"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="validator">The validator instance.</param>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task containing a <see cref="Result"/> indicating success or failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validator or instance is null.</exception>
    //==============================================================================================
    public static async Task<Result> ValidateAsResultAsync<T>(this Validator<T> validator, T instance, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(validator);

        var validationResult = await validator.ValidateAsync(instance, cancellationToken);
        return validationResult.ToResult();
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates an instance and returns a <see cref="Result{T}"/> with the value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="validator">The validator instance.</param>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task containing a <see cref="Result{T}"/> with the value if validation succeeded.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validator or instance is null.</exception>
    //==============================================================================================
    public static async Task<Result<T>> ValidateAsResultValueAsync<T>(this Validator<T> validator, T instance, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(validator);

        var validationResult = await validator.ValidateAsync(instance, cancellationToken);
        return validationResult.ToResult(instance);
    }

    //==============================================================================================
    /// <summary>
    /// Validates an instance and returns an <see cref="ErrorsResult"/> with individual errors.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="validator">The validator instance.</param>
    /// <param name="instance">The object instance to validate.</param>
    /// <returns>An <see cref="ErrorsResult"/> with all validation errors as separate Result failures.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validator or instance is null.</exception>
    //==============================================================================================
    public static ErrorsResult ValidateAsErrorsResult<T>(this Validator<T> validator, T instance)
    {
        ArgumentNullException.ThrowIfNull(validator);

        var validationResult = validator.Validate(instance);
        return validationResult.ToErrorsResult();
    }

    //==============================================================================================
    /// <summary>
    /// Asynchronously validates an instance and returns an <see cref="ErrorsResult"/> with individual errors.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="validator">The validator instance.</param>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task containing an <see cref="ErrorsResult"/> with all validation errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validator or instance is null.</exception>
    //==============================================================================================
    public static async Task<ErrorsResult> ValidateAsErrorsResultAsync<T>(this Validator<T> validator, T instance, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(validator);

        var validationResult = await validator.ValidateAsync(instance, cancellationToken);
        return validationResult.ToErrorsResult();
    }
}
