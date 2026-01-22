using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.UserSession
{
    public class userSession
    {
        public static string Email { get; private set; }
        public userSession(string email)
        {
            Email = email;
        }
        public string GetUserEmail()
        {
            return Email;
        }
    }
}
