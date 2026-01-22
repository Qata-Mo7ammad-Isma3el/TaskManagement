using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Services;

namespace TaskManagement.UserUI
{
    public class UserOrderUI
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        public UserOrderUI(
            CustomerService customerService,
            ProductService productService,
            OrderService orderService)
        {
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
        }
        public void Show(string email)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== ORDER MANAGEMENT ===");
                Console.WriteLine("1. Place New Order");
                Console.WriteLine("2. View My Orders");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        PlaceOrder(email);
                        break;
                    case "2":
                        ViewCustomerOrders(email);
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\n? Invalid option. Please try again.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        private void PlaceOrder(string email)
        {
            Console.Clear();
            Console.WriteLine("=== PLACE NEW ORDER ===");

            //Console.Write("Customer Email: ");
            //var email = Console.ReadLine();

            var customer = _customerService.GetCustomerByEmail(email);
            if (customer == null)
            {
                Console.WriteLine("Customer not found!");
                Console.WriteLine("Press any key to continue...");
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
                int productNum = Convert.ToInt32(Console.ReadLine());
                bool IsValidProductNum = int.TryParse(productNum.ToString(), out productNum);
                bool IsNullOrEmpty = string.IsNullOrEmpty(productNum.ToString());
                if (!IsValidProductNum || IsNullOrEmpty)
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
                Console.WriteLine("Press any key to continue...");
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
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        private void ViewCustomerOrders(string email)
        {
            Console.Clear();
            Console.WriteLine("=== VIEW CUSTOMER ORDERS ===");

            //Console.Write("Customer Email: ");
            //var email = Console.ReadLine();

            var orders = _orderService.GetCustomerOrders(email);

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                Console.WriteLine("Press any key to continue...");
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
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
