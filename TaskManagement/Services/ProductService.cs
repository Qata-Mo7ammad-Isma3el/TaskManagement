using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public Product CreateProduct(string name, string description, decimal price, int stock)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                StockQuantity = stock,
            };

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public List<Product> GetAllProducts(bool activeOnly = true)
        {
            var query = _context.Products.AsQueryable();

            if (activeOnly)
                query = query.Where(p => p.IsActive);

            return query.ToList();
        }

        public Product GetProductById(string id)
        {
            return _context.Products.Find(id);
        }

        public void UpdateProduct(string id, string name, string description, decimal price)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                product.Name = name;
                product.Description = description;
                product.Price = price;
                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }

        public void DeactivateProduct(string id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                product.IsActive = false;
                product.UpdatedAt = DateTime.UtcNow;
                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }
        public bool ReduceStock(string id, int quantity)
        {
            var product = GetProductById(id);
            if (product != null && quantity <= product.StockQuantity)
            {
                var oldStock = product.StockQuantity;
                product.StockQuantity -= quantity;
                product.UpdatedAt = DateTime.UtcNow;
                
                if (product.StockQuantity == 0)
                {
                    product.IsActive = false;
                }
                _context.Products.Update(product);             
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void IncreaseStock(string id, int quantity)
        {
            var product = GetProductById(id);
            if (product != null && quantity > 0)
            {
                product.StockQuantity += quantity;
                product.UpdatedAt = DateTime.UtcNow;
                _context.Products.Update(product);
                _context.SaveChanges();  
            }
        }
    }
}