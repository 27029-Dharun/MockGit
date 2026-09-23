using System.Text.RegularExpressions;

namespace CoffeeShop.Validators
{
    internal static class ViewValidator
    {
        public static bool IsValidEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email)) return false;

            string pattern = @"^[A-Za-z0-9.]+@[A-Za-z]+.[a-z]{2,}$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsValidPassword(string password)
        {
            if(string.IsNullOrWhiteSpace(password)) return false;

            return true;
            //string pattern = @"^{3, }$";

            //return Regex.IsMatch(password, pattern, RegexOptions.IgnoreCase);
        }
    }
}
