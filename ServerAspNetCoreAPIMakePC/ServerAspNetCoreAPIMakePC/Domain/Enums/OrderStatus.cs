namespace ServerAspNetCoreAPIMakePC.Domain.Enums;

/// <summary>
/// Represents the various statuses that an order can have in the MakePC application.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// The order has been placed but not yet processed.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// The order is currently being prepared or handled.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// The order has been shipped to the customer.
    /// </summary>
    Shipped = 3,

    /// <summary>
    /// The order has been delivered to the customer.
    /// </summary>
    Delivered = 4,

    /// <summary>
    /// The order has been cancelled and will not be fulfilled.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// The order has been refunded to the customer.
    /// </summary>
    Refunded = 6
}