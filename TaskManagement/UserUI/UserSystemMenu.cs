using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Data;
using TaskManagement.Services;
using TaskManagement.UserSession;

namespace TaskManagement.UserUI
{
    public class UserSystemMenu
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly userSession _userSession;
        private readonly AppDbContext _context;
        public UserSystemMenu(
            CustomerService customerService,
            ProductService productService,
            OrderService orderService,
            userSession userSession,
            AppDbContext context)
        {
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
            _userSession = userSession;
            _context = context;
        }
        public void Show()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== TASK MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Customer Operations");
                Console.WriteLine("2. Product Operations");
                Console.WriteLine("3. Order Operations");
                Console.WriteLine("4. Exit");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        new UserCustomerUI(_customerService).Show(_userSession.GetUserEmail());
                        break;
                    case "2":
                        new UserProductUI(_productService).Show();
                        break;
                    case "3":
                        new UserOrderUI(_customerService, _productService, _orderService).Show(_userSession.GetUserEmail());
                        break;
                    case "4":
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
    }
}
