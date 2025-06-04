using System;
using System.Collections.Generic;

namespace ECommerceApp
{
    public class ShoppingBasket2 : ProductPage
    {
        public static Dictionary<string, int> Basket = new Dictionary<string, int>();

        public static void ManageBasket()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Shopping Basket");
                Console.WriteLine("1: View Products");
                Console.WriteLine("2: Add Product to Basket");
                Console.WriteLine("3: View Basket");
                Console.WriteLine("4: Proceed to Checkout");
                Console.WriteLine("5: Back to Menu");
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
                        ProductPage.ViewProducts();
                        break;
                    case 2:
                        AddToBasket();
                        break;
                    case 3:
                        ViewBasket();
                        break;
                    case 4:
                        decimal totalPrice = CalculateTotalPrice();
                        Console.WriteLine($"Your total is: £{totalPrice:F2}");
                        Checkout checkout = new Checkout(totalPrice); // Create Checkout object with the total price
                        checkout.finish(); // Call the finish method to proc
                        return;
                    case 5:
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
            foreach (var item in Basket)
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

        public static void AddToBasket()
        {
            Console.Clear();
            Console.WriteLine("Available Products:");

            var productList = new List<(string ProductName, string Description, decimal Price, int Quantity)>();
            int productIndex = 1;

            // Build the product list for display
            foreach (var category in ProductPage.ProductsByCategory)
            {
                Console.WriteLine($"Category: {category.Key}");
                foreach (var product in category.Value)
                {
                    productList.Add(product);
                    Console.WriteLine($"{productIndex++}: {product.ProductName} - £{product.Price:F2} - Available: {product.Quantity}");
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

            if (quantity > chosenProduct.Quantity)
            {
                Console.WriteLine($"Sorry, we only have {chosenProduct.Quantity} of {chosenProduct.ProductName} available.");
                Console.ReadKey();
                return;
            }

            if (Basket.ContainsKey(chosenProduct.ProductName))
            {
                Basket[chosenProduct.ProductName] += quantity;
            }
            else
            {
                Basket[chosenProduct.ProductName] = quantity;
            }

            Console.WriteLine($"{quantity}x {chosenProduct.ProductName} added to your basket.");
            Console.ReadKey();
        }

        public static void ViewBasket()
        {
            Console.Clear();
            Console.WriteLine("Your Basket:");

            if (Basket.Count == 0)
            {
                Console.WriteLine("Your basket is empty.");
            }
            else
            {
                decimal total = 0;

                // Loop through each item in the basket and display details
                foreach (var item in Basket)
                {
                    foreach (var category in ProductPage.ProductsByCategory)
                    {
                        foreach (var product in category.Value)
                        {
                            if (product.ProductName.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                            {
                                decimal itemTotal = product.Price * item.Value;
                                total += itemTotal;
                                Console.WriteLine($"{item.Key} - Quantity: {item.Value} - Price: £{itemTotal:F2}");
                            }
                        }
                    }
                }

                Console.WriteLine($"Total Price: £{total:F2}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
