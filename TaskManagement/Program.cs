using System;
using TaskManagement.Data;
using TaskManagement.Services;
using TaskManagement.UI;

namespace TaskManagement
{
    class Program
    {
        static void Main()
        {
            using var context = new AppDbContext();

            context.Database.EnsureCreated();

            var customerService = new CustomerService(context);
            var productService = new ProductService(context);
            var orderService = new OrderService(context);

            var mainMenu = new MainMenu(
                customerService,
                productService,
                orderService,
                context
            );

            mainMenu.Show();
        }
    }
}
