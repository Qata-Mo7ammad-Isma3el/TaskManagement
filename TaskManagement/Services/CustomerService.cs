using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
    }

        public Customer CreateCustomer(string name, string email, string phoneNumber, string password)
        {
            if (_context.Customers.Any(c => c.Email == email))
                throw new Exception("Customer with this email already exists.");

            var customer = new Customer(name, email, phoneNumber, password);
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return customer;
        }

        public Customer GetCustomerByEmail(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.Email == email);
        }

        public Customer GetCustomerByEmailNoTracking(string email)
        {
            return _context.Customers.AsNoTracking().FirstOrDefault(c => c.Email == email);
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public void UpdateCustomer(string email, string name, string phoneNumber)
        {
            // Clear any tracked entities to prevent conflicts
            _context.ChangeTracker.Clear();
            
            var customer = GetCustomerByEmail(email);
            if (customer == null)
                throw new Exception("Customer not found.");

            customer.Name = name;
            customer.PhoneNumber = phoneNumber;
            
            // Explicitly mark as modified to ensure changes are saved
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void AddLoyaltyPoints(string email, int points)
        {
            // Clear any tracked entities to prevent conflicts
            _context.ChangeTracker.Clear();
            
            var customer = GetCustomerByEmail(email);
            if (customer != null)
            {
                customer.LoyaltyPoints += points;
                
                // Explicitly mark as modified to ensure changes are saved
                _context.Entry(customer).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeactivateCustomer(string email)
        {
            // Clear any tracked entities to prevent conflicts
            _context.ChangeTracker.Clear();
            
            var customer = GetCustomerByEmail(email);
            if (customer != null)
            {
                customer.IsActive = false;
                
                // Explicitly mark as modified to ensure changes are saved
                _context.Entry(customer).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public bool LoginCheck(string email, string password)
        {
            var customer = GetCustomerByEmail(email);
            if (customer != null && customer.Email == email && customer.VerifyPassword(password))
                return true;
            return false;
        }
    }
}