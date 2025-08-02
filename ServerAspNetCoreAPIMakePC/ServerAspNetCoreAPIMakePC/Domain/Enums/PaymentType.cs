namespace ServerAspNetCoreAPIMakePC.Domain.Enums;

/// <summary>
/// Represents the supported payment methods in the MakePC application.
/// </summary>
public enum PaymentType
{
    /// <summary>
    /// Payment made using a credit or debit card.
    /// </summary>
    Card = 1,

    /// <summary>
    /// Payment made using PayPal.
    /// </summary>
    PayPal = 2,

    /// <summary>
    /// Payment made via bank transfer.
    /// </summary>
    BankTransfer = 3,

    /// <summary>
    /// Payment made using cash on delivery.
    /// </summary>
    CashOnDelivery = 4,

    /// <summary>
    /// Payment made using a digital wallet (e.g. Apple Pay, Google Pay).
    /// </summary>
    DigitalWallet = 5,
}