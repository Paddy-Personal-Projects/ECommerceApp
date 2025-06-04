using ECommerceApp;
using System;
using System.Collections.Generic;

namespace ECommerceApp
{
    public class Login
    {
        public static List<string> _Users = new List<string>
        {
            "1, admin, admin, admin@admin.com, 0000000000, admin" // Hardcoded admin user
        };

        static Login()
        {

            var loadedUsers = DatabasePage.LoadUsers();
            _Users.AddRange(loadedUsers);
        }

        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.WriteLine("3: Exit");
                Console.WriteLine("Enter your choice:");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        LoginMethodInteractive();
                        break;
                    case 2:
                        RegisterInteractive();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static bool LoginMethod(string email, string password, out string role)
        {
            role = null;

            foreach (var user in _Users)
            {
                string[] userDetails = user.Split(',');
                if (userDetails[3].Trim().Equals(email, StringComparison.OrdinalIgnoreCase) &&
                    userDetails[2].Trim().Equals(password))
                {
                    role = userDetails[5].Trim().ToLower();
                    return true;
                }
            }

            return false;
        }

        private static void LoginMethodInteractive()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Login Page");
                Console.WriteLine("Enter your Email (or type 'back' to return to the Main Menu):");
                string email = Console.ReadLine();
                if (email?.ToLower() == "back") return;

                Console.WriteLine("Enter your Password:");
                string password = Console.ReadLine();

                if (LoginMethod(email, password, out string role))
                {
                    if (role == "admin")
                    {
                        Admin.AdminMenu();
                    }
                    else
                    {
                        Customer.Menu();
                    }
                    return;
                }

                Console.WriteLine("Invalid email or password. Please try again.");
                Console.ReadKey();
            }
        }

        public static bool Register(string name, string email, string password, string confirmPassword, int phone)
        {
            if (password != confirmPassword)
            {
                return false;
            }

            if (_Users.Exists(u => u.Split(',')[3].Trim().Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            int id = _Users.Count + 1;
            string newUser = $"{id}, {name}, {password}, {email}, {phone}, Customer";
            _Users.Add(newUser);
            DatabasePage.AddUser(newUser);

            return true;
        }

        private static void RegisterInteractive()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Register Page");
                Console.WriteLine("Enter your Name (or type 'back' to return to the Main Menu):");
                string name = Console.ReadLine();
                if (name?.ToLower() == "back") return;

                Random rnd = new Random();
                int id = rnd.Next(11111111, 99999999);
                Console.WriteLine("Enter your Email:");
                string email = Console.ReadLine();
                Console.WriteLine("Enter your Phone Number:");
                int phone = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter your Password:");
                string password = Console.ReadLine();
                Console.WriteLine("Confirm your Password:");
                string confirmPassword = Console.ReadLine();

                if (!Register(name, email, password, confirmPassword, phone))
                {
                    Console.WriteLine("Registration failed. Please try again.");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("User registered successfully!");
                Console.ReadKey();
                return;
            }
        }
    }
}
