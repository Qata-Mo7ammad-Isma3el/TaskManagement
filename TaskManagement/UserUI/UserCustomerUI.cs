using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Services;

namespace TaskManagement.UserUI
{
    public class UserCustomerUI
    {
        private readonly CustomerService _service;
        public UserCustomerUI(CustomerService service)
        {
            _service = service;
        }

        public void Show(string email)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== CUSTOMER MANAGEMENT ===");

                Console.WriteLine("1. Update My Info");
                Console.WriteLine("2. View My Info");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Select option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UpdateInfo(email);
                        break;
                    case "2":
                        ViewInfo(email);
                        break;
                    case "3":
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
        private void UpdateInfo(string email)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE MY INFO ===");
            //Console.Write("Enter your Email: ");
            //var email = Console.ReadLine();
            var customer = _service.GetCustomerByEmailNoTracking(email);
            if (customer == null)
            {
                Console.WriteLine("\n? Customer not found.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Current Name: {customer.Name}");
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
            else if (!string.IsNullOrWhiteSpace(newName) && string.IsNullOrWhiteSpace(newPhone))
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
        private void ViewInfo(string email)
        {
            Console.Clear();
            Console.WriteLine("=== VIEW MY INFO ===");
            //Console.Write("Enter your Email: ");
            //var email = Console.ReadLine();
            var customer = _service.GetCustomerByEmail(email);
            if (customer == null)
            {
                Console.WriteLine("\n? Customer not found.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine($"\nName: {customer.Name}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine($"Phone: {customer.PhoneNumber}");
            Console.WriteLine($"Loyalty Points: {customer.LoyaltyPoints}");
            Console.WriteLine($"\nPress any key to return back...");
            Console.ReadKey();
        }
    }
}
