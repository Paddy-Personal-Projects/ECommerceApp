using ECommerceApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceApp
{
    public class Admin : Login
    {
        public static void AdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Admin Menu");
                Console.WriteLine("1: Add Products");
                Console.WriteLine("2: View Products");
                Console.WriteLine("3: Update Product");
                Console.WriteLine("4: Remove Product");
                Console.WriteLine("5: View Registered Users");
                Console.WriteLine("6: Update User Details");
                Console.WriteLine("7: Logout");
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
                        AddProductInteractive();
                        break;
                    case 2:
                        ProductPage.ViewProducts();
                        break;
                    case 3:
                        UpdateProductInteractive();
                        break;
                    case 4:
                        RemoveProductInteractive();
                        break;
                    case 5:
                        ViewUsersInteractive();
                        break;
                    case 6:
                        UpdateUserDetailsInteractive();
                        break;
                    case 7:
                        Console.WriteLine("Logging out...");
                        Console.ReadKey();
                        Login.MainMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static void AddProductInteractive()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Add a new product");

                if (CategoryPage.Categories.Count == 0)
                {
                    Console.WriteLine("No categories available. Please add a category first.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Select a category:");
                for (int i = 0; i < CategoryPage.Categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {CategoryPage.Categories[i]}");
                }

                if (!int.TryParse(Console.ReadLine(), out int selectedCategory) || selectedCategory < 1 || selectedCategory > CategoryPage.Categories.Count)
                {
                    Console.WriteLine("Invalid category selection. Please try again.");
                    Console.ReadKey();
                    continue;
                }

                var category = CategoryPage.Categories[selectedCategory - 1];

                Console.Write("Enter Product Name: ");
                var productName = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Description: ");
                var description = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var price) || price <= 0)
                {
                    Console.WriteLine("Invalid price. Please try again.");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Enter Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity < 0)
                {
                    Console.WriteLine("Invalid quantity. Please try again.");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    Console.WriteLine(AddProduct(category, productName, description, price, quantity));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("Would you like to add another product? (yes/no)");
                string continueAdding = Console.ReadLine()?.Trim().ToLower();
                if (continueAdding != "yes")
                {
                    return;
                }
            }
        }

        public static void UpdateProductInteractive()
        {
            Console.Clear();
            Console.WriteLine("Update Product");

            if (ProductPage.ProductsByCategory.Count == 0)
            {
                Console.WriteLine("No products available to update.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Products:");
            var productList = new List<(string Category, int Index, string ProductName)>();
            int productIndex = 1;

            foreach (var category in ProductPage.ProductsByCategory)
            {
                Console.WriteLine($"Category: {category.Key}");
                foreach (var product in category.Value)
                {
                    Console.WriteLine($"{productIndex}: {product.ProductName}");
                    productList.Add((category.Key, productIndex, product.ProductName));
                    productIndex++;
                }
            }

            Console.WriteLine("Enter the number of the product you want to update:");
            if (!int.TryParse(Console.ReadLine(), out int selectedProduct) || selectedProduct < 1 || selectedProduct > productList.Count)
            {
                Console.WriteLine("Invalid selection. Please try again.");
                Console.ReadKey();
                return;
            }

            var chosenProduct = productList[selectedProduct - 1];

            Console.WriteLine("Enter new details for the product (or press Enter to keep current values):");

            Console.Write("New Product Name: ");
            string newName = Console.ReadLine()?.Trim();

            Console.Write("New Description: ");
            string newDescription = Console.ReadLine()?.Trim();

            Console.Write("New Price: ");
            decimal? newPrice = null;
            if (decimal.TryParse(Console.ReadLine(), out var tempPrice))
            {
                newPrice = tempPrice;
            }

            Console.Write("New Quantity: ");
            int? newQuantity = null;
            if (int.TryParse(Console.ReadLine(), out var tempQuantity))
            {
                newQuantity = tempQuantity;
            }

            if (UpdateProduct(chosenProduct.Category, chosenProduct.ProductName, newName, newDescription, newPrice, newQuantity))
            {
                Console.WriteLine("Product updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update product.");
            }

            Console.ReadKey();
        }

        public static void RemoveProductInteractive()
        {
            Console.Clear();
            Console.WriteLine("Remove Product");

            if (ProductPage.ProductsByCategory.Count == 0)
            {
                Console.WriteLine("No products available to remove.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Products:");
            var productList = new List<(string Category, int Index, string ProductName)>();
            int productIndex = 1;

            foreach (var category in ProductPage.ProductsByCategory)
            {
                Console.WriteLine($"Category: {category.Key}");
                foreach (var product in category.Value)
                {
                    Console.WriteLine($"{productIndex}: {product.ProductName}");
                    productList.Add((category.Key, productIndex, product.ProductName));
                    productIndex++;
                }
            }

            Console.WriteLine("Enter the number of the product you want to remove:");
            if (!int.TryParse(Console.ReadLine(), out int selectedProduct) || selectedProduct < 1 || selectedProduct > productList.Count)
            {
                Console.WriteLine("Invalid selection. Please try again.");
                Console.ReadKey();
                return;
            }

            var chosenProduct = productList[selectedProduct - 1];

            if (RemoveProduct(chosenProduct.Category, chosenProduct.ProductName))
            {
                Console.WriteLine($"Product '{chosenProduct.ProductName}' removed successfully!");
            }
            else
            {
                Console.WriteLine("Failed to remove product.");
            }

            Console.ReadKey();
        }

        public static void ViewUsersInteractive()
        {
            Console.Clear();
            Console.WriteLine("Registered Users:");

            foreach (var user in ViewUsers())
            {
                string[] userDetails = user.Split(',');
                Console.WriteLine($"ID: {userDetails[0].Trim()}, Name: {userDetails[1].Trim()}, Email: {userDetails[3].Trim()}, Role: {userDetails[5].Trim()}");
            }

            Console.WriteLine("Press any key to return to the Admin Menu...");
            Console.ReadKey();
        }

        public static void UpdateUserDetailsInteractive()
        {
            Console.Clear();
            Console.WriteLine("Update User Details:");
            Console.Write("Enter the email of the user to update (or type 'back' to return): ");
            string email = Console.ReadLine()?.Trim();

            if (email?.ToLower() == "back") return;

            Console.Write("Enter new name (or press Enter to keep current): ");
            string newName = Console.ReadLine();

            Console.Write("Enter new phone number (or press Enter to keep current): ");
            string newPhone = Console.ReadLine();

            Console.Write("Enter new role (admin or customer, or press Enter to keep current): ");
            string newRole = Console.ReadLine();

            if (UpdateUserDetails(email, newName, newPhone, newRole))
            {
                Console.WriteLine("User details updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update user details.");
            }

            Console.ReadKey();
        }

        public static string AddProduct(string category, string productName, string description, decimal price, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }

            if (!ProductPage.ProductsByCategory.ContainsKey(category))
            {
                ProductPage.ProductsByCategory[category] = new List<(string ProductName, string Description, decimal Price, int Quantity)>();
            }

            ProductPage.ProductsByCategory[category].Add((productName, description, price, quantity));
            DatabasePage.SaveProducts(ProductPage.ProductsByCategory);

            return $"Product '{productName}' added successfully!";
        }

        public static bool UpdateProduct(string category, string productName, string newName, string newDescription, decimal? newPrice, int? newQuantity)
        {
            if (!ProductPage.ProductsByCategory.ContainsKey(category))
                return false;

            var products = ProductPage.ProductsByCategory[category];
            var productIndex = products.FindIndex(p => p.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase));

            if (productIndex == -1)
                return false;

            var product = products[productIndex];

            var updatedProduct = (
                ProductName: string.IsNullOrWhiteSpace(newName) ? product.ProductName : newName,
                Description: string.IsNullOrWhiteSpace(newDescription) ? product.Description : newDescription,
                Price: newPrice ?? product.Price,
                Quantity: newQuantity ?? product.Quantity
            );

            products[productIndex] = updatedProduct;
            DatabasePage.SaveProducts(ProductPage.ProductsByCategory);

            return true;
        }

        public static bool RemoveProduct(string category, string productName)
        {
            if (!ProductPage.ProductsByCategory.ContainsKey(category))
                return false;

            var products = ProductPage.ProductsByCategory[category];
            var productIndex = products.FindIndex(p => p.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase));

            if (productIndex == -1)
                return false;

            products.RemoveAt(productIndex);
            DatabasePage.SaveProducts(ProductPage.ProductsByCategory);

            return true;
        }

        public static List<string> ViewUsers()
        {
            return Login._Users;
        }

        public static bool UpdateUserDetails(string email, string newName, string newPhone, string newRole)
        {
            var userIndex = Login._Users.FindIndex(u => u.Split(',')[3].Trim().Equals(email, StringComparison.OrdinalIgnoreCase));
            if (userIndex == -1)
                return false;

            var userDetails = Login._Users[userIndex].Split(',');

            string updatedUser = $"{userDetails[0]}, {(string.IsNullOrWhiteSpace(newName) ? userDetails[1] : newName)}, {userDetails[2].Trim()}, {email}, {(string.IsNullOrWhiteSpace(newPhone) ? userDetails[4] : newPhone)}, {(string.IsNullOrWhiteSpace(newRole) ? userDetails[5] : newRole)}";
            Login._Users[userIndex] = updatedUser;

            return true;
        }
    }
}
