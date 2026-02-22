//==================================================================================================
// Extension methods for converting ValidationResult to Fox.ResultKit Result types.
// Provides seamless integration between ValidationKit and ResultKit.
//==================================================================================================
using Fox.ResultKit;

namespace Fox.ValidationKit.ResultKit;

//==================================================================================================
/// <summary>
/// Extension methods for converting <see cref="ValidationResult"/> to <see cref="Result"/>.
/// </summary>
//==================================================================================================
public static class ValidationResultExtensions
{
    //==============================================================================================
    /// <summary>
    /// Converts a <see cref="ValidationResult"/> to a <see cref="Result"/>.
    /// </summary>
    /// <param name="validationResult">The validation result to convert.</param>
    /// <returns>A <see cref="Result"/> representing success or failure with validation errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validationResult is null.</exception>
    //==============================================================================================
    public static Result ToResult(this ValidationResult validationResult)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

        if (validationResult.IsValid)
        {
            return Result.Success();
        }

        var errorMessages = validationResult.Errors.Select(e => e.ErrorCode != null ? $"{e.ErrorCode}: {e.Message}" : $"{e.PropertyName}: {e.Message}");
        var combinedMessage = string.Join("; ", errorMessages);

        return Result.Failure(combinedMessage);
    }

    //==============================================================================================
    /// <summary>
    /// Converts a <see cref="ValidationResult"/> to a <see cref="Result{T}"/> with a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="validationResult">The validation result to convert.</param>
    /// <param name="value">The value to include in the success result.</param>
    /// <returns>A <see cref="Result{T}"/> with the value if validation succeeded, otherwise failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validationResult is null.</exception>
    //==============================================================================================
    public static Result<T> ToResult<T>(this ValidationResult validationResult, T value)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

        if (validationResult.IsValid)
        {
            return Result<T>.Success(value);
        }

        var errorMessages = validationResult.Errors.Select(e => e.ErrorCode != null ? $"{e.ErrorCode}: {e.Message}" : $"{e.PropertyName}: {e.Message}");
        var combinedMessage = string.Join("; ", errorMessages);

        return Result<T>.Failure(combinedMessage);
    }

    //==============================================================================================
    /// <summary>
    /// Converts a <see cref="ValidationResult"/> to an <see cref="ErrorsResult"/> with individual errors.
    /// </summary>
    /// <param name="validationResult">The validation result to convert.</param>
    /// <returns>An <see cref="ErrorsResult"/> with all validation errors as separate Result failures.</returns>
    /// <exception cref="ArgumentNullException">Thrown when validationResult is null.</exception>
    //==============================================================================================
    public static ErrorsResult ToErrorsResult(this ValidationResult validationResult)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

        if (validationResult.IsValid)
        {
            return ErrorsResult.Success();
        }

        var results = validationResult.Errors.Select(e =>
        {
            var message = e.ErrorCode != null ? $"{e.ErrorCode}: {e.Message}" : $"{e.PropertyName}: {e.Message}";
            return Result.Failure(message);
        }).Cast<IResult>().ToArray();

        return ErrorsResult.Collect(results);
    }
}
