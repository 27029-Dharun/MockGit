namespace CoffeeShop.Repository;

internal class Logger
{
    private readonly string _path;
    //private object lockObject = new object();

    public Logger(string path)
    {
        _path = path;
        if (!File.Exists(_path))
        {
            File.WriteAllText(path, "");
        }

    }

    public void LogText(string log)
    {
        {
            File.AppendAllText(_path, log);
        }
    }
}
