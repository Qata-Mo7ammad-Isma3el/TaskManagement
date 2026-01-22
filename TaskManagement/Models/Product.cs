using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }  // Changed from ProductName to Name
        public string Description { get; set; }
        public decimal Price { get; set; }  // Changed from double to decimal
        public int StockQuantity { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Product() { }

        public Product(string name, string description, decimal price, int stockQuantity)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - ${Price:F2} | Stock: {StockQuantity}";
        }
    }
}