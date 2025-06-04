using System;
using System.Collections.Generic;

namespace ECommerceApp
{
    public class CategoryPage
    {
        public static List<string> Categories = new List<string>
        {
            "Food",
            "Electronics",
            "Drinks",
            "Clothes",
            "Other"
        };

        public static void ManageCategories()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Category Management");
                Console.WriteLine("1: Add Category");
                Console.WriteLine("2: View Categories");
                Console.WriteLine("3: Back to Menu");
                Console.WriteLine("Enter your choice:");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddCategoryInteractive();
                        break;
                    case 2:
                        ViewCategories();
                        break;
                    case 3:
                        return; // Exit to previous menu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public static string AddCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return "Category name cannot be empty or whitespace!";
            }

            if (Categories.Exists(c => c.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return $"Category '{categoryName}' already exists!";
            }

            Categories.Add(categoryName);
            return $"Category '{categoryName}' added successfully!";
        }

        private static void AddCategoryInteractive()
        {
            Console.Clear();
            Console.WriteLine("Enter category name:");
            string categoryName = Console.ReadLine();

            string result = AddCategory(categoryName);
            Console.WriteLine(result);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ViewCategories()
        {
            Console.Clear();
            Console.WriteLine("Categories:");
            if (Categories.Count == 0)
            {
                Console.WriteLine("No categories available.");
            }
            else
            {
                for (int i = 0; i < Categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {Categories[i]}");
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}