using CoffeeShopApp.Enums;

namespace CoffeeShopApp.Models;

/// <summary>
/// Represents a notification produced by the application.
/// </summary>
public sealed class Notification
{
    /// <summary>Gets or sets the notification type.</summary>
    public NotificationType Type { get; set; }

    /// <summary>Gets or sets the notification message.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>Gets or sets the creation time.</summary>
    public DateTime CreatedAt { get; set; }
}
