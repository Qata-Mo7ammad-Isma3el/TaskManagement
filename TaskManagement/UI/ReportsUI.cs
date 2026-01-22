using System;
using System.Linq;
using TaskManagement.Data;

namespace TaskManagement.UI
{
    public class ReportsUI
    {
        private readonly AppDbContext _context;

        public ReportsUI(AppDbContext context)
        {
            _context = context;
        }

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== REPORTS ===\n");

            var totalCustomers = _context.Customers.Count();
            var totalProducts = _context.Products.Count();
            var totalOrders = _context.Orders.Count();
            var totalRevenue = _context.Orders.Sum(o => o.TotalAmount);

            Console.WriteLine($"Total Customers : {totalCustomers}");
            Console.WriteLine($"Total Products  : {totalProducts}");
            Console.WriteLine($"Total Orders    : {totalOrders}");
            Console.WriteLine($"Total Revenue   : ${totalRevenue:F2}");

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
