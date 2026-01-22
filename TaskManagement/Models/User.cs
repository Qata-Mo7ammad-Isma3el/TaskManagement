using System;

namespace TaskManagement.Models
{
    public abstract class User
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        protected User() { }

        protected User(string name, string email, string phoneNumber, string password)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password); // Proper hashing
        }

        public virtual bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }
}