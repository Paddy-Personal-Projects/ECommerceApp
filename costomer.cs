using System;
using System.Collections.Generic;

namespace ECommerceApp
{
    public class Customer : Login
    {
        public static void Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Customer Menu");
                Console.WriteLine("1: View Products");
                Console.WriteLine("2: Add Product to Shopping Basket");
                Console.WriteLine("3: View Shopping Basket");
                Console.WriteLine("4: Proceed to Checkout");
                Console.WriteLine("5: Update Profile");
                Console.WriteLine("6: Logout");
                Console.WriteLine("Enter your choice:");

                if (!int.TryParse(Console.ReadLine(), out int input))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.ReadKey();
                    continue;
                }

                switch (input)
                {
                    case 1:
                        ProductPage.ViewProducts();
                        break;
                    case 2:
                        AddProductToBasket();
                        break;
                    case 3:
                        ShoppingBasket2.ViewBasket();
                        break;
                    case 4:
                        // Calculate the total price of items in the basket
                        decimal totalPrice = CalculateTotalPrice();
                        Console.WriteLine($"Your total is: £{totalPrice:F2}");

                        // Navigate to Checkout
                        Checkout checkout = new Checkout(totalPrice);
                        checkout.finish(); // Proceed with checkout process
                        return; // Exit or return to the menu after checkout is completed
                    case 5:
                        UpdateProfile();
                        break;
                    case 6:
                        Logout();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public static decimal CalculateTotalPrice()
        {
            decimal total = 0;

            // Loop through each item in the basket and calculate the total price
            foreach (var item in ShoppingBasket2.Basket)
            {
                foreach (var category in ProductPage.ProductsByCategory)
                {
                    foreach (var product in category.Value)
                    {
                        if (product.ProductName.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            total += product.Price * item.Value;
                        }
                    }
                }
            }

            return total;
        }
        private static void AddProductToBasket()
        {
            Console.Clear();
            Console.WriteLine("Available Products:");

            var productList = new List<(string ProductName, string Description, decimal Price, int Quantity)>();
            int productIndex = 1;

            foreach (var category in ProductPage.ProductsByCategory)
            {
                Console.WriteLine($"Category: {category.Key}");
                foreach (var product in category.Value)
                {
                    productList.Add(product);
                    Console.WriteLine($"{productIndex++}: {product.ProductName} - £{product.Price:F2}");
                    Console.WriteLine($"   Description: {product.Description}");
                    Console.WriteLine($"   Quantity Available: {product.Quantity}");
                }
            }

            if (productList.Count == 0)
            {
                Console.WriteLine("No products available.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter the number of the product you want to add to your basket:");
            if (!int.TryParse(Console.ReadLine(), out int selectedProduct) || selectedProduct < 1 || selectedProduct > productList.Count)
            {
                Console.WriteLine("Invalid selection. Please try again.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter the quantity:");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity. Please try again.");
                Console.ReadKey();
                return;
            }

            var chosenProduct = productList[selectedProduct - 1];

            // Check if the requested quantity exceeds available stock
            if (quantity > chosenProduct.Quantity)
            {
                Console.WriteLine($"Insufficient stock. Only {chosenProduct.Quantity} available.");
                Console.ReadKey();
                return;
            }

            if (ShoppingBasket2.Basket.ContainsKey(chosenProduct.ProductName))
            {
                ShoppingBasket2.Basket[chosenProduct.ProductName] += quantity;
            }
            else
            {
                ShoppingBasket2.Basket[chosenProduct.ProductName] = quantity;
            }

            // Update stock in ProductPage
            chosenProduct.Quantity -= quantity;

            Console.WriteLine($"{quantity}x {chosenProduct.ProductName} added to your basket.");
            Console.ReadKey();
        }

        public static void UpdateProfile()
        {
            Console.Clear();
            Console.WriteLine("Update your profile details:");
            Console.WriteLine("Enter your Email:");
            string userEmail = Console.ReadLine();

            var user = _Users.Find(u => u.Contains(userEmail));
            if (user == null)
            {
                Console.WriteLine("User not found.");
                Console.ReadKey();
                return;
            }

            string[] userDetails = user.Split(',');

            string currentName = userDetails[1].Trim();
            string currentPassword = userDetails[2].Trim();
            string currentPhone = userDetails[4].Trim();

            Console.WriteLine($"Current Name: {currentName}");
            Console.WriteLine("Enter new name (or press Enter to keep current):");
            string newName = Console.ReadLine();
            if (string.IsNullOrEmpty(newName)) newName = currentName;

            Console.WriteLine("Enter new password (or press Enter to keep current):");
            string newPassword = Console.ReadLine();
            if (string.IsNullOrEmpty(newPassword)) newPassword = currentPassword;

            Console.WriteLine($"Current Phone: {currentPhone}");
            Console.WriteLine("Enter new phone number (or press Enter to keep current):");
            string newPhone = Console.ReadLine();
            if (string.IsNullOrEmpty(newPhone)) newPhone = currentPhone;

            string updatedUser = $"{userDetails[0]}, {newName}, {newPassword}, {userDetails[3].Trim()}, {newPhone}, {userDetails[5].Trim()}";

            int userIndex = _Users.IndexOf(user);
            _Users[userIndex] = updatedUser;

            Console.WriteLine("Profile updated successfully!");
            Console.ReadKey();
        }

        public static void Logout()
        {
            Console.Clear();
            Login.MainMenu();
        }
    }
}