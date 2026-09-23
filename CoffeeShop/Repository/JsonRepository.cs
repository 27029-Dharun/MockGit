using System.Text.Json;

namespace CoffeeShop.Repository
{
    internal static class JsonRepository
    {/// <summary>
     /// Options to write and read the file
     /// </summary>
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        public static void Save<T>(string path, List<T> elements)
        {
            string json = JsonSerializer.Serialize(elements, options);

            File.WriteAllText(path, json);
        }

        public static List<T> Load<T>(string path)
        {
            if (!File.Exists(path))
            {
                // create an empty file
                File.WriteAllText(path, "[]");
            }

            string? json = File.ReadAllText(path);

            if(string.IsNullOrWhiteSpace(json))
            {
                return new List<T> { };
            }

            return JsonSerializer.Deserialize<List<T>>(json, options) ?? new List<T>();
        }
    }
}
