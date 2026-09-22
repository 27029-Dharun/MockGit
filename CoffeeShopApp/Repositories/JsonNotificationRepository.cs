using System.Text.Json;
using CoffeeShopApp.Models;

namespace CoffeeShopApp.Repositories;

/// <summary>
/// Stores notifications in a JSON file.
/// </summary>
public sealed class JsonNotificationRepository : INotificationRepository
{
    private readonly string _filePath;
    private readonly object _lock = new();

    /// <summary>
    /// Initializes the JSON notification repository.
    /// </summary>
    /// <param name="filePath">JSON file path.</param>
    public JsonNotificationRepository(string filePath)
    {
        _filePath = filePath;
    }

    /// <inheritdoc />
    public void Save(Notification notification)
    {
        lock (_lock)
        {
            List<Notification> notifications = Load();
            notifications.Add(notification);
            Write(notifications);
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<Notification> GetAll()
    {
        lock (_lock)
        {
            return Load().AsReadOnly();
        }
    }

    private List<Notification> Load()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<Notification>>(json) ?? [];
    }

    private void Write(List<Notification> notifications)
    {
        string json = JsonSerializer.Serialize(
            notifications,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(_filePath, json);
    }
}
