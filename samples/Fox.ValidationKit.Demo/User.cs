//==================================================================================================
// User model for demonstration purposes.
// Simple POCO class with validation-ready properties.
//==================================================================================================

namespace Fox.ValidationKit.Demo;

//==================================================================================================
/// <summary>
/// User model for demonstration.
/// </summary>
//==================================================================================================
public sealed class User
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
    public string? PhoneNumber { get; set; }
}
