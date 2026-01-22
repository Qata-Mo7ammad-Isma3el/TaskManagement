using System;
using TaskManagement.Data;
using TaskManagement.Services;

namespace TaskManagement.UI
{
    public class MainMenu
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly AppDbContext _context;

        public MainMenu(
            CustomerService customerService,
            ProductService productService,
            OrderService orderService,
            AppDbContext context)
        {
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
            _context = context;
        }

        public void Show()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== TASK MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Select option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;

                    case "2":
                        Register();
                        break;

                    case "3":
                        exit = true;
                        break;
                }
            }
        }

        private void Login()
        {
            Console.Clear();
            Console.WriteLine("=== LOGIN ===");

            Console.Write("Email: ");
            var email = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            if (_customerService.LoginCheck(email, password))
            {
                new SystemMenu(
                    _customerService,
                    _productService,
                    _orderService,
                    _context
                ).Show();
            }
            else
            {
                Console.WriteLine("Invalid credentials.");
                Console.ReadKey();
            }
        }

        private void Register()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTER ===");

            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Email: ");
            var email = Console.ReadLine();

            Console.Write("Phone: ");
            var phone = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            try
            {
                _customerService.CreateCustomer(name, email, phone, password);

                Console.WriteLine("\nRegistration successful!");
                Console.ReadKey();

                new SystemMenu(
                    _customerService,
                    _productService,
                    _orderService,
                    _context
                ).Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
