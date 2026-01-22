using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }  // Changed from TotalPrice
        public decimal TaxAmount { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal FinalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order() { }

        public Order(string customerId)
        {
            CustomerId = customerId;
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            if (!product.IsActive)
                throw new InvalidOperationException("Product is not active");

            if (quantity > product.StockQuantity)
                throw new InvalidOperationException($"Insufficient stock. Available: {product.StockQuantity}");

            var existingItem = OrderItems.FirstOrDefault(oi => oi.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.CalculateSubtotal();
            }
            else
            {
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };
                orderItem.CalculateSubtotal();
                OrderItems.Add(orderItem);
            }

            CalculateTotals();
        }

        public void RemoveItem(string productId)
        {
            var item = OrderItems.FirstOrDefault(oi => oi.ProductId == productId);
            if (item != null)
            {
                OrderItems.Remove(item);
                CalculateTotals();
            }
        }

        public void UpdateItemQuantity(string productId, int newQuantity)
        {
            var item = OrderItems.FirstOrDefault(oi => oi.ProductId == productId);
            if (item != null)
            {
                item.Quantity = newQuantity;
                item.CalculateSubtotal();
                CalculateTotals();
            }
        }

        public void CalculateTotals()
        {
            TotalAmount = OrderItems.Sum(oi => oi.Subtotal);
            FinalAmount = TotalAmount + TaxAmount - DiscountAmount;
        }

        public void ApplyDiscount(decimal discount)
        {
            if (discount < 0 || discount > TotalAmount)
                throw new ArgumentException("Invalid discount amount");

            DiscountAmount = discount;
            CalculateTotals();
        }

        public void ApplyLoyaltyPoints(int points, decimal pointValue = 0.1m)
        {
            DiscountAmount += points * pointValue;
            CalculateTotals();
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;

            if (newStatus == OrderStatus.Shipped)
                ShippedDate = DateTime.UtcNow;
            else if (newStatus == OrderStatus.Delivered)
                DeliveredDate = DateTime.UtcNow;
        }

        public bool CanCancel()
        {
            return Status == OrderStatus.Pending || Status == OrderStatus.Processing;
        }
    }
}