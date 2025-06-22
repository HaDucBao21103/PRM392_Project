using System.Text.RegularExpressions;

namespace Vaccine.API.Helper
{
    public static class Utils
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^0[1-9]\d{8}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
