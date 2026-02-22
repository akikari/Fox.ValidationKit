//==================================================================================================
// Represents a single validation error with property name and error message.
// Immutable record type for validation error representation.
//==================================================================================================

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Represents a validation error for a specific property with an associated error message.
/// </summary>
//==================================================================================================
public sealed record ValidationError
{
    #region Properties

    //==============================================================================================
    /// <summary>
    /// Gets the name of the property that failed validation.
    /// </summary>
    //==============================================================================================
    public string PropertyName { get; }

    //==============================================================================================
    /// <summary>
    /// Gets the validation error message describing why the validation failed.
    /// </summary>
    //==============================================================================================
    public string Message { get; }

    //==============================================================================================
    /// <summary>
    /// Gets the error code identifying the type of validation failure.
    /// </summary>
    //==============================================================================================
    public string? ErrorCode { get; }

    #endregion

    #region Constructors

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property that failed validation.</param>
    /// <param name="message">The validation error message.</param>
    /// <param name="errorCode">The error code identifying the validation failure.</param>
    /// <exception cref="ArgumentNullException">Thrown when propertyName or message is null.</exception>
    //==============================================================================================
    public ValidationError(string propertyName, string message, string? errorCode = null)
    {
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        ErrorCode = errorCode;
    }

    #endregion
}
