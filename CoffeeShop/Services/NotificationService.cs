namespace CoffeeShop.Services;

internal class NotificationService
{
    public delegate void Notifier(string message, int userId);

    public event Notifier? DisplayNotification;

    public void Execute(string message, int userId)
    {
        DisplayNotification?.Invoke(message, userId);
    }
}
