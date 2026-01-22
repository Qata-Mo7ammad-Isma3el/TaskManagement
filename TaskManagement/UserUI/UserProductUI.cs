using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Services;

namespace TaskManagement.UserUI
{
    public class UserProductUI
    {
        private readonly ProductService _service;

        public UserProductUI(ProductService service)
        {
            _service = service;
        }

        public void Show()
        {
            bool exit = false;
            while (!exit) {
                Console.Clear();
                Console.WriteLine("=== PRODUCT MANAGEMENT ===");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Back to Main Menu");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        var products = _service.GetAllProducts();
                        foreach (var p in products)
                            Console.WriteLine($"{p.Name} - {p.Price} - Stock:{p.StockQuantity}");
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();

                        break;
                    case "2":
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
