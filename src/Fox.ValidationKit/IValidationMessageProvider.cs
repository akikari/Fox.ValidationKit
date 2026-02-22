//==================================================================================================
// Provides localized or custom validation error messages based on error codes.
// Allows applications to override default error messages with localized or custom versions.
//==================================================================================================

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Provides localized or custom validation error messages based on error codes.
/// </summary>
//==================================================================================================
public interface IValidationMessageProvider
{
    //==============================================================================================
    /// <summary>
    /// Gets the validation error message for a specific error code and property.
    /// </summary>
    /// <param name="errorCode">The error code identifying the validation failure.</param>
    /// <param name="propertyName">The name of the property that failed validation.</param>
    /// <param name="args">Optional arguments for message formatting (e.g., min length, max length).</param>
    /// <returns>The formatted error message.</returns>
    //==============================================================================================
    string GetMessage(string errorCode, string propertyName, params object[] args);
}
