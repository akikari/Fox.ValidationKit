//==================================================================================================
// Represents the result of a validation operation containing validation status and errors.
// Provides factory methods for creating success and failure results.
//==================================================================================================

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Represents the result of a validation operation, containing validation status and errors.
/// </summary>
//==================================================================================================
public sealed class ValidationResult
{
    #region Fields

    private readonly List<ValidationError> errors;

    #endregion

    #region Constructors

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResult"/> class.
    /// </summary>
    //==============================================================================================
    private ValidationResult()
    {
        errors = [];
    }

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResult"/> class with errors.
    /// </summary>
    /// <param name="errors">The collection of validation errors.</param>
    //==============================================================================================
    private ValidationResult(IEnumerable<ValidationError> errors)
    {
        this.errors = [.. errors ?? throw new ArgumentNullException(nameof(errors))];
    }

    #endregion

    #region Properties

    //==============================================================================================
    /// <summary>
    /// Gets a value indicating whether the validation was successful (no errors).
    /// </summary>
    //==============================================================================================
    public bool IsValid => errors.Count == 0;

    //==============================================================================================
    /// <summary>
    /// Gets the collection of validation errors. Empty if validation was successful.
    /// </summary>
    //==============================================================================================
    public IReadOnlyList<ValidationError> Errors => errors.AsReadOnly();

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Creates a successful validation result with no errors.
    /// </summary>
    /// <returns>A successful <see cref="ValidationResult"/>.</returns>
    //==============================================================================================
    public static ValidationResult Success() => new();

    //==============================================================================================
    /// <summary>
    /// Creates a failed validation result with the specified errors.
    /// </summary>
    /// <param name="errors">The collection of validation errors.</param>
    /// <returns>A failed <see cref="ValidationResult"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when errors is null.</exception>
    //==============================================================================================
    public static ValidationResult Failure(IEnumerable<ValidationError> errors) => new(errors);

    //==============================================================================================
    /// <summary>
    /// Creates a failed validation result with a single error.
    /// </summary>
    /// <param name="propertyName">The name of the property that failed validation.</param>
    /// <param name="message">The validation error message.</param>
    /// <returns>A failed <see cref="ValidationResult"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when propertyName or message is null.</exception>
    //==============================================================================================
    public static ValidationResult Failure(string propertyName, string message)
    {
        return new ValidationResult([new ValidationError(propertyName, message)]);
    }

    #endregion

    #region Internal Methods

    //==============================================================================================
    /// <summary>
    /// Adds a validation error to this result.
    /// </summary>
    /// <param name="error">The validation error to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when error is null.</exception>
    //==============================================================================================
    internal void AddError(ValidationError error)
    {
        errors.Add(error ?? throw new ArgumentNullException(nameof(error)));
    }

    #endregion
}
