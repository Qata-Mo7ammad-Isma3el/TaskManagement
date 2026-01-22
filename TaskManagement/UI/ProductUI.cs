using TaskManagement.Services;

namespace TaskManagement.UI
{
    public class ProductUI
    {
        private readonly ProductService _service;

        public ProductUI(ProductService service)
        {
            _service = service;
        }

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== PRODUCT MANAGEMENT ===");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View Products");
            Console.Write("Select option: ");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Name: ");
                var name = Console.ReadLine();

                Console.Write("Description: ");
                var desc = Console.ReadLine();

                Console.Write("Stock: ");
                var stock = int.Parse(Console.ReadLine());

                Console.Write("Price: ");
                var price = decimal.Parse(Console.ReadLine());

                _service.CreateProduct(name, desc, price, stock);
            }
            else if (choice == "2")
            {
                var products = _service.GetAllProducts();
                foreach (var p in products)
                    Console.WriteLine($"{p.Name} - {p.Price} - Stock:{p.StockQuantity}");

                Console.ReadKey();
            }
        }
    }
}
