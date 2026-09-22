namespace CoffeeShopTasks.Services;

internal class Notify
{
    public delegate void Notifier(string message);

    public event Notifier? DisplayNotification;

    public void Execute(string message)
    {
        this.DisplayNotification?.Invoke(message);
    }
}
