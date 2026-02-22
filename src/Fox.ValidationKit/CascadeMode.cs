//==================================================================================================
// Enum defining how validation should cascade when multiple rules are applied.
// Controls whether validation continues or stops after the first failure.
//==================================================================================================

namespace Fox.ValidationKit;

//==================================================================================================
/// <summary>
/// Specifies how validation should cascade when multiple rules are applied to a property.
/// </summary>
//==================================================================================================
public enum CascadeMode
{
    //==============================================================================================
    /// <summary>
    /// Continue validating all rules even if one fails (collect all errors).
    /// </summary>
    //==============================================================================================
    Continue,

    //==============================================================================================
    /// <summary>
    /// Stop validating remaining rules after the first failure (fail fast).
    /// </summary>
    //==============================================================================================
    Stop
}
