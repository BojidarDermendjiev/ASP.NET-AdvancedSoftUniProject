namespace ServerAspNetCoreAPIMakePC.Domain.Enums;

/// <summary>
/// Represents possible roles for a user in the MakePC application.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// The standard user with regular permissions.
    /// </summary>
    User = 0,

    /// <summary>
    /// The administrator with elevated permissions.
    /// </summary>
    Admin = 1,

    /// <summary>
    /// The moderator with permissions to manage content or users.
    /// </summary>
    Moderator = 2,

    /// <summary>
    /// The guest user with limited access.
    /// </summary>
    Guest = 3
}