using CoffeeShopApp.Models;

namespace CoffeeShopApp.Repositories;

/// <summary>
/// Provides persistence operations for notifications.
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Saves a notification.
    /// </summary>
    /// <param name="notification">Notification to save.</param>
    void Save(Notification notification);

    /// <summary>
    /// Gets all saved notifications.
    /// </summary>
    /// <returns>Saved notification snapshot.</returns>
    IReadOnlyList<Notification> GetAll();
}
