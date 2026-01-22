using TaskManagement.Data;
using TaskManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public Order PlaceOrder(string customerEmail, Dictionary<string, int> cart)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Email == customerEmail);
            if (customer == null)
                throw new Exception("Customer not found.");

            var order = new Order
            {
                CustomerId = customer.UserId,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in cart)
            {
                var product = _context.Products.Find(item.Key);
                if (product == null)
                    throw new Exception($"Product {item.Key} not found.");

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Value,
                    UnitPrice = product.Price
                };

                // Calculate subtotal for the order item
                orderItem.CalculateSubtotal();

                order.OrderItems.Add(orderItem);
                total += orderItem.Subtotal;
            }

            order.TotalAmount = total;
            order.TaxAmount = total * 0.07m;
            order.ShippedDate = DateTime.UtcNow.AddDays(2);
            order.DeliveredDate = DateTime.UtcNow.AddDays(5);
            order.FinalAmount = total - order.DiscountAmount + order.TaxAmount;
            order.Status = OrderStatus.Pending;

            // Add loyalty points (1 point per $10 spent)
            var pointsEarned = (int)(total / 10);
            customer.LoyaltyPoints += pointsEarned;
            _context.Customers.Update(customer);
            _context.SaveChanges();

            _context.Orders.Add(order);
            _context.SaveChanges();
            Console.WriteLine($"Order placed successfully! Total: ${total:F2}");
            Console.WriteLine($"Customer earned {pointsEarned} loyalty points.");

            return order;
        }

        public List<Order> GetCustomerOrders(string email)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            if (customer == null) return new List<Order>();

            return _context.Orders
                .Where(o => o.CustomerId == customer.UserId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public void UpdateOrderStatus(string orderId, OrderStatus status)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }
        }
    }
}