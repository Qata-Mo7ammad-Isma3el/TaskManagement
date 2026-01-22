using System.Text.RegularExpressions;

namespace TaskManagement.Utilities
{
    public static class Validators
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Accepts +962XXXXXXXXX or 07XXXXXXXX
            var regex = new Regex(@"^(\+962|0)[1-9]\d{7,8}$");
            return regex.IsMatch(phoneNumber);
        }

        public static bool IsValidPassword(string password)
        {
            // At least 8 chars, 1 uppercase, 1 lowercase, 1 number
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
            return regex.IsMatch(password);
        }
    }
}