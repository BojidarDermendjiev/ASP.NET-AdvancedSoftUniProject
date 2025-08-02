namespace ServerAspNetCoreAPIMakePC.Domain.Enums;

/// <summary>
/// Represents the possible statuses for a payment in the MakePC application.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// The payment has not yet been initiated.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// The payment is currently being processed.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// The payment was successful.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// The payment failed due to an error or rejection.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// The payment was cancelled by the user or system.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// The payment was refunded after completion.
    /// </summary>
    Refunded = 6
}