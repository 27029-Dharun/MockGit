using CoffeeShop.Models.Enums;
using CoffeeShop.Validators;

namespace CoffeeShop.Views;

/// <summary>
/// Contains the console operations that prints and gets input from user.
/// </summary>
public class ConsoleView
{
    /// <summary>
    /// Prints the input string.
    /// </summary>
    /// <param name="message">The string to be printed.</param>
    public void PrintInfo(string message)
    {
        Console.WriteLine(message);
    }

    /// <summary>
    /// Gets the main menu option from the user.
    /// </summary>
    /// <returns>The menu option entered by the user.</returns>
    public CoffeeMenu GetMainMenuOption()
    {
        string menuMessage = "       COFFEE SHOP - MAIN MENU       \n" +
               "[1] Espresso\n" +
               "[2] Cappuccino\n" +
               "[3] Latte\n" +
               "[4] Americano\n" +
               "[5] Exit Application\n\n" +
               "Please enter your choice (1-5): ";

        return GetEnumValue<CoffeeMenu>(menuMessage);
    }

    /// <summary>
    /// Displays the enum value and gets input from the user.
    /// </summary>
    /// <typeparam name="T">Type variable that should be struct.</typeparam>
    /// <param name="message">String to be printed.</param>
    /// <returns>returns a enum value entered by use.</returns>
    public T GetEnumValue<T>(string message)
       where T : struct, Enum
    {
        while (true)
        {
            string input = GetString(message);
            if (Enum.TryParse(input, out T result) && Enum.IsDefined(result))
            {
                return result;
            }

            Console.Clear();
            Console.WriteLine("Enter a valid option");
        }
    }

    /// <summary>
    /// Clears the console messages.
    /// </summary>
    public void ClearConsole()
    {
        // Erases the entire scroll back buffer history
        Console.Write("\x1b[3J");
        Console.Clear();
    }

    /// <summary>
    /// Displays the error message in red color.
    /// </summary>
    /// <param name="message">message to be printed.</param>
    public void PrintError(string message)
    {
        PrintColoredText(message, ConsoleColor.Red);
    }

    /// <summary>
    /// Displays the success message in green color.
    /// </summary>
    /// <param name="message">message to be printed.</param>
    public void PrintSuccess(string message)
    {
        PrintColoredText(message, ConsoleColor.Green);
    }

    /// <summary>
    /// Displays the error message in red color.
    /// </summary>
    /// <param name="message">message to be printed.</param>
    public void PrintWarning(string message)
    {
        PrintColoredText(message, ConsoleColor.Yellow);
    }

    /// <summary>
    /// Waits for user to press a key and clears the console.
    /// </summary>
    public void PauseAndReturn()
    {
        Console.WriteLine("Press any key to return to main menu");
        Console.ReadKey();

        ClearConsole();
    }

    private void PrintColoredText(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    internal string GetEmail()
    {
        return GetValidatedInput("Enter your email address: ", ViewValidator.IsValidEmail, "Please enter a valid email");
    }

    internal string GetPassword()
    {
        return GetValidatedInput("Enter the password: ", ViewValidator.IsValidPassword, "Password must consists atleast 8 characters.");
    }

    private string GetValidatedInput(string prompt, Func<string, bool> isValidEmail, string errorMessage)
    {
        int attemptsLeft = 3;
        while (attemptsLeft >= 0)
        {
            string input = this.GetString(prompt);
            if (isValidEmail(input)) return input;
            attemptsLeft--;

            Console.WriteLine(errorMessage + $"Tries left: {attemptsLeft}");
        }

        throw new InvalidDataException(errorMessage);
    }

    /// <summary>
    /// Gets the string input from the user.
    /// </summary>
    /// <param name="message">Message to be printed.</param>
    /// <returns>int value that we got as input.</returns>
    private string GetString(string message)
    {
        Console.Write(message);
        string input = (Console.ReadLine() ?? string.Empty).Trim();

        return input;
    }
}
