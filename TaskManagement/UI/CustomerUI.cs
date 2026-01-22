using TaskManagement.Services;

namespace TaskManagement.UI
{
    public class CustomerUI
    {
        private readonly CustomerService _service;

        public CustomerUI(CustomerService service)
        {
            _service = service;
        }

        public void Show()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== CUSTOMER MANAGEMENT ===");
                Console.WriteLine("1. Create New Customer");
                Console.WriteLine("2. View All Customers");
                Console.WriteLine("3. Update Customer");
                Console.WriteLine("4. Add Loyalty Points");
                Console.WriteLine("5. Deactivate Customer");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Select option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateCustomer();
                        break;
                    case "2":
                        ViewAllCustomers();
                        break;
                    case "3":
                        UpdateCustomer();
                        break;
                    case "4":
                        AddLoyaltyPoints();
                        break;
                    case "5":
                        DeactivateCustomer();
                        break;
                    case "6":
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

        private void CreateCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== CREATE NEW CUSTOMER ===\n");

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
                var customer = _service.CreateCustomer(name, email, phone, password);
                Console.WriteLine("\n? Customer created successfully!");
                Console.WriteLine($"User ID: {customer.UserId}");
                Console.WriteLine($"Name: {customer.Name}");
                Console.WriteLine($"Email: {customer.Email}");
                Console.WriteLine($"Phone: {customer.PhoneNumber}");
                Console.WriteLine("\nPress any key to return back...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n? Error: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }


        }

        private void ViewAllCustomers()
        {
            Console.Clear();
            Console.WriteLine("=== VIEW ALL CUSTOMERS ===\n");

            var customers = _service.GetAllCustomers();

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                Console.WriteLine("\nPress any key to return back...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Total Customers: {customers.Count}\n");
                Console.WriteLine(new string('-', 120));
                Console.WriteLine($"{"Name",-25} {"Email",-30} {"Phone",-15} {"Loyalty",-10} {"Status",-10} {"Created",-20}");
                Console.WriteLine(new string('-', 120));

                foreach (var customer in customers)
                {
                    var status = customer.IsActive ? "Active" : "Inactive";
                    Console.WriteLine($"{customer.Name,-25} {customer.Email,-30} {customer.PhoneNumber,-15} {customer.LoyaltyPoints,-10} {status,-10} {customer.CreatedAt:yyyy-MM-dd HH:mm}");
                }

                Console.WriteLine(new string('-', 120));
                Console.WriteLine("\nPress any key to return back...");
                Console.ReadKey();
            }

        }

        private void UpdateCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE CUSTOMER ===\n");

            Console.Write("Customer Email: ");
            var email = Console.ReadLine();

            var customer = _service.GetCustomerByEmailNoTracking(email);
            if (customer == null)
            {
                Console.WriteLine("\n? Customer not found!");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nCurrent Name: {customer.Name}");
            Console.Write("New Name (press Enter to keep current): ");
            var newName = Console.ReadLine();

            Console.WriteLine($"Current Phone: {customer.PhoneNumber}");
            Console.Write("New Phone (press Enter to keep current): ");
            var newPhone = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newName) && string.IsNullOrWhiteSpace(newPhone))
            {
                Console.WriteLine("\n? No changes made.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            else if(!string.IsNullOrWhiteSpace(newName) && string.IsNullOrWhiteSpace(newPhone))
            {
                newPhone = customer.PhoneNumber;
            }
            else if (string.IsNullOrWhiteSpace(newName) && !string.IsNullOrWhiteSpace(newPhone))
            {
                newName = customer.Name;
            }
            // else: both have values, use the entered values (newName and newPhone)



                try
                {
                    _service.UpdateCustomer(email, newName, newPhone);
                    Console.WriteLine("\n? Customer updated successfully!");
                    Console.WriteLine($"Name: {newName}");
                    Console.WriteLine($"Phone: {newPhone}");
                    Console.WriteLine("\nPress any key to return back...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n? Error: {ex.Message}");
                    Console.WriteLine("\nPress any key to return back...");
                    Console.ReadKey();
                }
        }

        private void AddLoyaltyPoints()
        {
            Console.Clear();
            Console.WriteLine("=== ADD LOYALTY POINTS ===\n");

            Console.Write("Customer Email: ");
            var email = Console.ReadLine();

            var customer = _service.GetCustomerByEmail(email);
            if (customer == null)
            {
                Console.WriteLine("\n? Customer not found!");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nCustomer: {customer.Name}");
            Console.WriteLine($"Current Loyalty Points: {customer.LoyaltyPoints}");
            Console.Write("\nPoints to Add: ");

            if (int.TryParse(Console.ReadLine(), out int points) && points > 0)
            {
                try
                {
                    _service.AddLoyaltyPoints(email, points);
                    Console.WriteLine($"\n? Added {points} loyalty points successfully!");
                    Console.WriteLine($"New Total: {customer.LoyaltyPoints + points} points");
                    Console.WriteLine("\nPress any key to return back...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n? Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("\n? Invalid points value. Must be a positive number.");
                Console.ReadKey();
            }


        }

        private void DeactivateCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== DEACTIVATE CUSTOMER ===\n");

            Console.Write("Customer Email: ");
            var email = Console.ReadLine();

            var customer = _service.GetCustomerByEmail(email);
            if (customer == null)
            {
                Console.WriteLine("\n? Customer not found!");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            if (!customer.IsActive)
            {
                Console.WriteLine($"\n? Customer '{customer.Name}' is already inactive.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nCustomer: {customer.Name}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.Write("\nAre you sure you want to deactivate this customer? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();

            if (confirm == "y" || confirm == "yes")
            {
                try
                {
                    _service.DeactivateCustomer(email);
                    Console.WriteLine("\n? Customer deactivated successfully!");
                    Console.WriteLine("\nPress any key to return back...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n? Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("\n? Deactivation cancelled.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }


        }
    }
}