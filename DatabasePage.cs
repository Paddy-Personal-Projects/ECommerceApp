using System;
using System.Collections.Generic;
using System.IO;

namespace ECommerceApp
{
    public class DatabasePage
    {
        // Change private to internal to allow testing access
        public static readonly string UserFilePath = "UserData.txt";
        public static readonly string ProductFilePath = "ProductsData.txt";

        // Load user data from the file at the start
        public static List<string> LoadUsers()
        {
            var users = new List<string>();
            if (File.Exists(UserFilePath))
            {
                users.AddRange(File.ReadAllLines(UserFilePath));
            }
            return users;
        }

        // Save user data to the file
        public static void SaveUsers(List<string> users)
        {
            File.WriteAllLines(UserFilePath, users);
        }

        // Add a single user to the file
        public static void AddUser(string user)
        {
            using (var sw = File.AppendText(UserFilePath))
            {
                sw.WriteLine(user);
            }
        }

        // Load product data from the file at the start
        public static Dictionary<string, List<(string ProductName, string Description, decimal Price, int Quantity)>> LoadProducts()
        {
            var products = new Dictionary<string, List<(string ProductName, string Description, decimal Price, int Quantity)>>();

            if (File.Exists(ProductFilePath))
            {
                var lines = File.ReadAllLines(ProductFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 5 &&
                        !string.IsNullOrWhiteSpace(parts[0]) &&
                        !string.IsNullOrWhiteSpace(parts[1]) &&
                        decimal.TryParse(parts[3], out var price) &&
                        int.TryParse(parts[4], out var quantity))
                    {
                        var category = parts[0].Trim();
                        var productName = parts[1].Trim();
                        var description = parts[2].Trim();

                        if (!products.ContainsKey(category))
                        {
                            products[category] = new List<(string ProductName, string Description, decimal Price, int Quantity)>();
                        }

                        products[category].Add((productName, description, price, quantity));
                    }
                }
            }

            return products;
        }

        // Save all product data to the file
        public static void SaveProducts(Dictionary<string, List<(string ProductName, string Description, decimal Price, int Quantity)>> products)
        {
            var lines = new List<string>();
            foreach (var category in products)
            {
                foreach (var product in category.Value)
                {
                    lines.Add($"{category.Key} | {product.ProductName} | {product.Description} | {product.Price:F2} | {product.Quantity}");
                }
            }
            File.WriteAllLines(ProductFilePath, lines);
        }

        // Add a single product to the file
        public static void AddProduct(string category, string productName, string description, decimal price, int quantity)
        {
            using (var sw = File.AppendText(ProductFilePath))
            {
                sw.WriteLine($"{category} | {productName} | {description} | {price:F2} | {quantity}");
            }
        }
    }
}
