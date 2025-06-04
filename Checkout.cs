using System;
using System.Collections.Generic;

namespace ECommerceApp
{
    public class Checkout : ShoppingBasket2
    {
        private decimal totalPrice = 0;

        public Checkout(decimal price)
        {
            totalPrice = price;
        }

       public void finish()
        {

            DeliveryAddress();
            ShippingType();
            TakePayment();
        }

        public void DeliveryAddress()
        {
            Console.WriteLine("Enter house number:");
            int houseNumb = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter street name:");
            string streetName = Console.ReadLine();
            Console.WriteLine("Enter postcode:");
            string postcode = Console.ReadLine();
            Console.WriteLine($"Your address is: {houseNumb} {streetName} {postcode}. Enter Y to confirm or any other key to exit.");
            char confirm = Console.ReadKey().KeyChar;

            if (houseNumb != 0 && !string.IsNullOrEmpty(streetName) && !string.IsNullOrEmpty(postcode) && (confirm == 'Y' || confirm == 'y'))
            {
                Console.WriteLine(" Delivery address accepted.");
            }
            else
            {
                Console.WriteLine(" Delivery address not accepted.");
            }
        }

        public void ShippingType()
        {
            Console.WriteLine("Enter 1 for express shipping or 2 for regular shipping.");
            int userChoice = Convert.ToInt32(Console.ReadLine());

            if (userChoice == 1)
            {
                Console.WriteLine("Thank you for choosing express shipping. Your price will be: £" + (totalPrice + 5)); // Add express shipping cost
                totalPrice += 5;
            }
            else if (userChoice == 2)
            {
                Console.WriteLine("Thank you for choosing regular shipping. Your price will be: £" + (totalPrice + 2)); // Add regular shipping cost
                totalPrice += 2;
            }
            else
            {
                Console.WriteLine("Invalid choice. Press 0 to exit or any other key to retry.");
                char retry = Console.ReadKey().KeyChar;
                if (retry == '0')
                {
                    return;
                }
                else
                {
                    ShippingType();
                }
            }
        }

        public void TakePayment()
        {
            // Card number input validation
            string cardNumber = "";
            while (cardNumber.Length != 16 || !long.TryParse(cardNumber, out _))
            {
                Console.WriteLine("Enter 16 digit card number:");
                cardNumber = Console.ReadLine();
                if (cardNumber.Length != 16 || !long.TryParse(cardNumber, out _))
                {
                    Console.WriteLine("Invalid card number. Please enter exactly 16 digits.");
                }
            }

            // CVC input validation
            string cvc = "";
            while (cvc.Length != 3 || !int.TryParse(cvc, out _))
            {
                Console.WriteLine("Enter CVC number (3 digits):");
                cvc = Console.ReadLine();
                if (cvc.Length != 3 || !int.TryParse(cvc, out _))
                {
                    Console.WriteLine("Invalid CVC. Please enter exactly 3 digits.");
                }
            }

            // Expiry date input validation
            Console.WriteLine("Enter expiry date (MM/YY):");
            string expDate = Console.ReadLine();

            Console.WriteLine("Your details are: ");
            Console.WriteLine($"Card Number: {cardNumber} CVC: {cvc} Expiry Date: {expDate}");

            Console.WriteLine("Enter Y to confirm, or any other key to quit.");
            char confirm = Console.ReadKey().KeyChar;
            if (confirm == 'Y' || confirm == 'y')
            {
                Console.WriteLine("Your details have been confirmed.");
            }
            else
            {
                Console.WriteLine("Details failed to be confirmed. Press 0 to exit or any other key to retry.");
                char retry = Console.ReadKey().KeyChar;
                if (retry == '0')
                {
                    return;
                }
                else
                {
                    TakePayment();
                }
            }

            // Confirming the payment and transaction
            Console.WriteLine($"You are going to buy product for £{totalPrice} enter Y to confirm, or any other key to quit.");
            confirm = Console.ReadKey().KeyChar;
            if (confirm == 'Y' || confirm == 'y')
            {
                Console.WriteLine("Transaction completed. Thank you.");
            }
            else
            {
                Console.WriteLine("Transaction incomplete. Press 0 to exit or any other key to retry.");
                char retry = Console.ReadKey().KeyChar;
                if (retry == '0')
                {
                    return;
                }
                else
                {
                    TakePayment();
                }
            }
        }
    }
}
