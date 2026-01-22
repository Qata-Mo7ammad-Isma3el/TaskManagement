using System;
using System.Collections.Generic;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.UI
{
    public class OrderUI
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;

        public OrderUI(
            CustomerService customerService,
            ProductService productService,
            OrderService orderService)
        {
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
        }

        public void Show()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== ORDER MANAGEMENT ===");
                Console.WriteLine("1. Place New Order");
                Console.WriteLine("2. View Customer Orders");
                Console.WriteLine("3. Update Order Status");
                Console.WriteLine("4. Back");
                Console.Write("Select option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PlaceOrder();
                        break;

                    case "2":
                        ViewCustomerOrders();
                        break;

                    case "3":
                        UpdateOrderStatus();
                        break;

                    case "4":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // =========================
        // PLACE NEW ORDER
        // =========================
        private void PlaceOrder()
        {
            Console.Clear();
            Console.WriteLine("=== PLACE NEW ORDER ===");

            Console.Write("Customer Email: ");
            var email = Console.ReadLine();

            var customer = _customerService.GetCustomerByEmail(email);
            if (customer == null)
            {
                Console.WriteLine("Customer not found!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nOrdering for: {customer.Name}");
            Console.WriteLine("\nAvailable Products:");

            var products = _productService.GetAllProducts();
            for (int i = 0; i < products.Count; i++)
            {
                var p = products[i];
                Console.WriteLine($"{i + 1}. {p.Name} - ${p.Price:F2} (Stock: {p.StockQuantity})");
            }

            var cart = new Dictionary<string, int>();
            bool adding = true;

            while (adding)
            {
                Console.Write("\nEnter product number (0 to finish): ");
                if (!int.TryParse(Console.ReadLine(), out int productNum))
                    continue;

                if (productNum == 0)
                {
                    adding = false;
                    continue;
                }

                if (productNum < 1 || productNum > products.Count)
                {
                    Console.WriteLine("Invalid product number.");
                    continue;
                }

                var product = products[productNum - 1];

                Console.Write($"Enter quantity for {product.Name}: ");
                if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
                    continue;

                if (qty > product.StockQuantity)
                {
                    Console.WriteLine($"Insufficient stock! Available: {product.StockQuantity}");
                    continue;
                }

                if (cart.ContainsKey(product.Id))
                    cart[product.Id] += qty;
                else
                    cart[product.Id] = qty;

                _productService.ReduceStock(product.Id, qty);
                Console.WriteLine($"Added {qty} x {product.Name} to cart");
            }

            if (cart.Count == 0)
            {
                Console.WriteLine("\nNo items in cart. Order cancelled.");
                Console.ReadKey();
                return;
            }

            try
            {
                var order = _orderService.PlaceOrder(email, cart);
                Console.WriteLine($"\n✓ Order placed successfully!");
                Console.WriteLine($"Order ID: {order.OrderId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error placing order: {ex.Message}");
            }

            Console.ReadKey();
        }

        // =========================
        // VIEW CUSTOMER ORDERS
        // =========================
        private void ViewCustomerOrders()
        {
            Console.Clear();
            Console.WriteLine("=== VIEW CUSTOMER ORDERS ===");

            Console.Write("Customer Email: ");
            var email = Console.ReadLine();

            var orders = _orderService.GetCustomerOrders(email);

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                Console.ReadKey();
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"\nOrder ID: {order.OrderId}");
                Console.WriteLine($"Date: {order.OrderDate:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Status: {order.Status}");
                Console.WriteLine($"Total: ${order.TotalAmount:F2}");
                Console.WriteLine("Items:");

                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine(
                        $" - {item.Product?.Name ?? "Unknown"} x{item.Quantity} " +
                        $"@ ${item.UnitPrice:F2} = ${item.Subtotal:F2}");
                }

                Console.WriteLine(new string('-', 40));
            }

            Console.ReadKey();
        }

        // =========================
        // UPDATE ORDER STATUS
        // =========================
        private void UpdateOrderStatus()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE ORDER STATUS ===");

            Console.Write("Order ID: ");
            var orderId = Console.ReadLine();

            Console.WriteLine("\n1. Pending");
            Console.WriteLine("2. Processing");
            Console.WriteLine("3. Shipped");
            Console.WriteLine("4. Delivered");
            Console.WriteLine("5. Cancelled");
            Console.Write("Select status: ");

            var choice = Console.ReadLine();

            OrderStatus status = choice switch
            {
                "1" => OrderStatus.Pending,
                "2" => OrderStatus.Processing,
                "3" => OrderStatus.Shipped,
                "4" => OrderStatus.Delivered,
                "5" => OrderStatus.Cancelled,
                _ => throw new Exception("Invalid status")
            };

            try
            {
                _orderService.UpdateOrderStatus(orderId, status);
                Console.WriteLine($"✓ Status updated to {status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
