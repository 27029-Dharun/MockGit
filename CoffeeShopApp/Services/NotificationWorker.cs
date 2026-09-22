using CoffeeShopApp.Models;

namespace CoffeeShopApp.Services;

/// <summary>
/// Consumes notifications on a dedicated thread.
/// </summary>
public sealed class NotificationWorker : IDisposable
{
    private readonly NotificationService _notificationService;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Thread? _thread;

    /// <summary>
    /// Initializes the notification worker.
    /// </summary>
    /// <param name="notificationService">Notification service.</param>
    public NotificationWorker(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Starts the worker thread.
    /// </summary>
    public void Start()
    {
        if (_thread is not null)
        {
            return;
        }

        _thread = new Thread(Run)
        {
            IsBackground = true,
            Name = "NotificationWorker"
        };

        _thread.Start();
    }

    private void Run()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            Notification? notification =
                _notificationService.WaitForNotification(
                    _cancellationTokenSource.Token);

            if (notification is null)
            {
                continue;
            }

            lock (Console.Out)
            {
                Console.WriteLine(
                    $"[NOTIFICATION] {notification.CreatedAt:HH:mm:ss} " +
                    $"{notification.Message}");
            }
        }
    }

    /// <summary>
    /// Stops the worker and releases resources.
    /// </summary>
    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _thread?.Join();
        _cancellationTokenSource.Dispose();
    }
}
