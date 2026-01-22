using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.UI
{
    public class SystemMenu
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly AppDbContext _context;

        public SystemMenu(
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
                Console.WriteLine("1. Customer Operations");
                Console.WriteLine("2. Product Operations");
                Console.WriteLine("3. Order Operations");
                Console.WriteLine("4. View Reports");
                Console.WriteLine("5. Exit");
                Console.Write("Select option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        new CustomerUI(_customerService).Show();
                        break;

                    case "2":
                        new ProductUI(_productService).Show();
                        break;

                    case "3":
                        new OrderUI(_customerService, _productService, _orderService).Show();
                        break;

                    case "4":
                        new ReportsUI(_context).Show();
                        break;

                    case "5":
                        exit = true;
                        break;
                }
            }
        }
    }
}
