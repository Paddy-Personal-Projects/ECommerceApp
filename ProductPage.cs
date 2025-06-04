using ECommerceApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApp
{
    public class ProductPage : CategoryPage
    {
        public static Dictionary<string, List<(string ProductName, string Description, decimal Price, int Quantity)>> ProductsByCategory = DatabasePage.LoadProducts();

        public static void ManageProducts()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Product Management");
                Console.WriteLine("1: Add Product");
                Console.WriteLine("2: View Products by Category");
                Console.WriteLine("3: Back to Admin Menu");
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
                        AddProduct();
                        break;
                    case 2:
                        ViewProducts();
                        break;
                    case 3:
                        return; // Exit to the Admin menu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public static void AddProduct()
        {
            Console.Clear();

            if (CategoryPage.Categories.Count == 0)
            {
                Console.WriteLine("No categories available. Please add a category first.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Select a category:");
            for (int i = 0; i < CategoryPage.Categories.Count; i++)
                Console.WriteLine($"{i + 1}: {CategoryPage.Categories[i]}");

            if (!int.TryParse(Console.ReadLine(), out int selectedCategory) || selectedCategory < 1 || selectedCategory > CategoryPage.Categories.Count)
            {
                Console.WriteLine("Invalid category selection.");
                Console.ReadKey();
                return;
            }

            string category = CategoryPage.Categories[selectedCategory - 1];

            Console.Write("Enter Product Name: ");
            string productName = Console.ReadLine()?.Trim();

            Console.Write("Enter Description: ");
            string description = Console.ReadLine()?.Trim();

            // Use a local variable for price instead of the property
            decimal price;
            Console.Write("Enter Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
            {
                Console.WriteLine("Invalid price. Please try again.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Invalid quantity. Please try again.");
                Console.ReadKey();
                return;
            }

            // Initialize the category entry if it doesn't exist
            if (!ProductsByCategory.ContainsKey(category))
                ProductsByCategory[category] = new List<(string ProductName, string Description, decimal Price, int Quantity)>();

            // Add the product to the list
            ProductsByCategory[category].Add((productName, description, price, quantity));

            // Save the updated product list to the database
            DatabasePage.SaveProducts(ProductsByCategory);

            Console.WriteLine("Product added successfully!");
            Console.ReadKey();
        }

        public static string ViewProducts(bool isTestEnvironment = false)
        {
            if (!isTestEnvironment)
            {
                Console.Clear();
            }

            if (ProductsByCategory.Count == 0)
            {
                string noProductsMessage = "No products available.";
                if (!isTestEnvironment) Console.WriteLine(noProductsMessage);
                return noProductsMessage;
            }

            var output = new StringBuilder();
            foreach (var category in ProductsByCategory)
            {
                output.AppendLine($"Category: {category.Key}");
                foreach (var product in category.Value)
                {
                    output.AppendLine($"  - Name: {product.ProductName}");
                    output.AppendLine($"    Description: {product.Description}");
                    output.AppendLine($"    Price: £{product.Price:F2}");
                    output.AppendLine($"    Quantity: {product.Quantity}");
                }
            }

            if (!isTestEnvironment)
            {
                Console.WriteLine(output.ToString());
            }

            return output.ToString();
        }
    }
}
