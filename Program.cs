//==========================================================
// Student Number : S10275174
// Student Name : Ang Zheng Yang
// Partner Name : Hayden Soh Kai Jun
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using S10275174_PRG2Assignment;

namespace S10275174_PRG2Assignment
{
    class Program
    {
        static List<Restaurant> restaurants = new List<Restaurant>();
        static List<Customer> customers = new List<Customer>();
        static List<Order> orders = new List<Order>();

        // ==============================================
        // Start Of Program
        // By Zheng Yang
        // ==============================================
        static void Main(string[] args)
        {
            LoadRestaurants();
            LoadFoodItems();
            LoadCustomers();
            LoadOrders();

            Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
            Console.WriteLine($"{restaurants.Count} restaurants loaded!");
            Console.WriteLine($"{restaurants.Sum(r => r.FoodItems.Count)} food items loaded!");
            Console.WriteLine($"{customers.Count} customers loaded!");
            Console.WriteLine($"{orders.Count} orders loaded!");
            Console.WriteLine();

            while (true)
            {
                DisplayMenu();
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListRestaurantsAndMenus();
                        break;
                    case "2":
                        ListAllOrders();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ==============================================
        // Main Menu
        // By Zheng Yang
        // ==============================================
        static void DisplayMenu()
        {
            Console.WriteLine("==== Gruberoo Food Delivery System ====");
            Console.WriteLine("1. List all restaurants and menu items");
            Console.WriteLine("2. List all orders");
            Console.WriteLine("0. Exit");
        }

        // ==============================================
        // Question 1 – Load Files
        // By Zheng Yang
        // ==============================================
        static void LoadRestaurants()
        {
            string[] lines = File.ReadAllLines("restaurants.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                restaurants.Add(new Restaurant(parts[0], parts[1], parts[2]));
            }
        }

        static void LoadFoodItems()
        {
            string[] lines = File.ReadAllLines("fooditems.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                string restId = parts[0];

                Restaurant r = restaurants.FirstOrDefault(x => x.RestaurantId == restId);
                if (r != null)
                {
                    FoodItem item = new FoodItem(
                        parts[1],
                        parts[2],
                        double.Parse(parts[3]),
                        restId
                    );
                    r.AddFoodItem(item);
                }
            }
        }

        static void LoadCustomers()
        {
            string[] lines = File.ReadAllLines("customers.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                customers.Add(new Customer(parts[0], parts[1]));
            }
        }

        static void LoadOrders()
        {
            string[] lines = File.ReadAllLines("orders.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');

                int orderId = int.Parse(parts[0]);
                Customer c = customers.First(x => x.EmailAddress == parts[1]);
                Restaurant r = restaurants.First(x => x.RestaurantId == parts[2]);

                Order o = new Order(orderId, c, r);
                o.DeliveryDateTime = DateTime.Parse($"{parts[3]} {parts[4]}");
                o.OrderTotal = double.Parse(parts[7]);
                o.Status = Enum.Parse<OrderStatus>(parts[8]);

                orders.Add(o);
            }
        }

        // ==============================================
        // Question 4 – List All Orders
        // By Zheng Yang
        // ==============================================
        static void ListAllOrders()
        {
            Console.WriteLine("\nAll Orders\n==========");
            Console.WriteLine("Order ID  Customer        Restaurant        Delivery Date/Time     Amount    Status");
            Console.WriteLine("--------  ----------      -------------     ------------------     ------    ---------");

            foreach (Order o in orders)
            {
                Console.WriteLine(
                    $"{o.OrderId,-9} " +
                    $"{o.Customer.CustomerName,-15} " +
                    $"{o.Restaurant.RestaurantName,-17} " +
                    $"{o.DeliveryDateTime,-22:dd/MM/yyyy HH:mm} " +
                    $"${o.OrderTotal,-8:F2} " +
                    $"{o.Status}"
                );
            }

            Console.WriteLine();
        }

        // =======================
        // Helper
        // By Zheng Yang
        // =======================
        static void ListRestaurantsAndMenus()
        {
            foreach (Restaurant r in restaurants)
            {
                Console.WriteLine($"\n{r.RestaurantName}");
                foreach (FoodItem f in r.FoodItems)
                    Console.WriteLine($"- {f.ItemName} (${f.ItemPrice:F2})");
            }
            Console.WriteLine();
        }
    }
}