using System.Collections.Generic;

namespace TaskManagement.Models
{
    public class Customer : User
    {
        public int LoyaltyPoints { get; set; } = 0;

        // Navigation Properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public Customer() : base() { }

        public Customer(string name, string email, string phoneNumber, string password)
            : base(name, email, phoneNumber, password)
        {
        }

        public void AddLoyaltyPoints(int points)
        {
            if (points > 0)
            {
                LoyaltyPoints += points;
            }
        }

        public bool UseLoyaltyPoints(int points)
        {
            if (points > 0 && points <= LoyaltyPoints)
            {
                LoyaltyPoints -= points;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"ID: {UserId} | {Name} | Email: {Email} | Points: {LoyaltyPoints}";
        }
    }
}