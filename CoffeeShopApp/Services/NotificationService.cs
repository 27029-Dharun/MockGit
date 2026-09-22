using CoffeeShopApp.Enums;
using CoffeeShopApp.Models;
using CoffeeShopApp.Repositories;

namespace CoffeeShopApp.Services;

/// <summary>
/// Manages the in-memory notification queue and persistence.
/// </summary>
public sealed class NotificationService
{
    private readonly Queue<Notification> _queue = new();
    private readonly object _lock = new();
    private readonly AutoResetEvent _notificationEvent = new(false);
    private readonly INotificationRepository _repository;

    /// <summary>
    /// Initializes the notification service.
    /// </summary>
    /// <param name="repository">Notification repository.</param>
    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Publishes a notification.
    /// </summary>
    /// <param name="type">Notification category.</param>
    /// <param name="message">Notification message.</param>
    public void Publish(NotificationType type, string message)
    {
        var notification = new Notification
        {
            Type = type,
            Message = message,
            CreatedAt = DateTime.Now
        };

        lock (_lock)
        {
            _queue.Enqueue(notification);
            _repository.Save(notification);
        }

        _notificationEvent.Set();
    }

    /// <summary>
    /// Waits for the next queued notification.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Notification or null when cancelled.</returns>
    public Notification? WaitForNotification(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            lock (_lock)
            {
                if (_queue.Count > 0)
                {
                    return _queue.Dequeue();
                }
            }

            _notificationEvent.WaitOne(250);
        }

        return null;
    }
}
