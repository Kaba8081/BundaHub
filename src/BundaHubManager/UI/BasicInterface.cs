using Domain.Models;
using BundaHubManager.Services.Interfaces;
using BundaHubManager.UI.Interfaces;
using Domain.Entites;

namespace BundaHubManager.UI
{
    public class BasicInterface : IInterface
    {
        private IManager _manager;
        private static string[] _menuItems = new string[]
        {
                "View inventory",
                "Add item",
                "Search",
                "Make reservation",
                "View reservations",
                "Exit"
        };

        public BasicInterface(IManager manager)
        {
            _manager = manager;
        }

        private static int GetInput(int max, int min = 0)
        {
            int input;
            while (true)
            {
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    if (input >= min && input <= max)
                    {
                        return input;
                    }
                }
                Console.WriteLine($"Invalid input. ({min} - {max})");
            }
        }

        private static void DisplayResult(bool status, string message)
        {
            if (status)
            {
                Console.WriteLine($"Operation successful. {message}");
                return;
            }

            Console.WriteLine($"Operation failed {message}");
            return;
        }

        private static void DisplaySearchResults(List<ItemModel> results)
        {
            if (results.Count > 0)
            {
                Console.WriteLine("Search Results:");
                foreach (var item in results)
                {
                    Console.WriteLine($"{item.Name}, Price: {item.Price}, Quantity: {item.Quantity}, Total Price: {item.TotalPrice}");
                }
            }
            else
            {
                Console.WriteLine("No items found.");
            }
            Console.WriteLine(" ");
        }

        public void ViewInventory()
        {
            var inventory = _manager.GetInventory();

            int[] colSizes = {
                    inventory.Max(i => i.Name.Length),
                    inventory.Max(i => i.Price.ToString().Length),
                    inventory.Max(i => i.Quantity.ToString().Length),
                    inventory.Max(i => i.TotalPrice.ToString().Length),
                    inventory.Max(i => String.Join(", ", i.GetProperties).Length),
                };

            Console.WriteLine($"{"Name".PadRight(colSizes[0])} | {"Price".PadRight(colSizes[1])} | {"Quantity".PadRight(colSizes[2])} | {"Total Price".PadRight(colSizes[3])} | {"Properties".PadRight(colSizes[4])}");
            Console.WriteLine(new string('-', colSizes.Sum() + colSizes.Length * 3 - 1));

            foreach (var item in inventory)
            {
                Console.WriteLine($"{item.Name.PadRight(colSizes[0])} | {item.Price.ToString().PadRight(colSizes[1])} | {item.Quantity.ToString().PadRight(colSizes[2])} | {item.TotalPrice.ToString().PadRight(colSizes[3])} | {String.Join(", ", item.GetProperties).PadRight(colSizes[4])}");
            }
        }

        public void AddItem()
        {
            Console.Write("Enter item name: ");
            string name = Console.ReadLine();

            Console.Write("Enter item price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Enter item quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            Console.Write("Enter item properties (comma separated): ");
            string[] properties = Console.ReadLine().Split(',');

            var newItem = new ItemModel(name, price, quantity, properties.Select(p => Enum.Parse<ItemProperties>(p.Trim().ToUpper())).ToArray());
            var (status, message) = _manager.AddItem(newItem);

            DisplayResult(status, message);
        }

        public void Search()
        {
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine();

            var results = _manager.GetInventory().Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            DisplaySearchResults(results);
        }

        public void AddReservation()
        {
            Console.Write("Enter item name: ");
            string itemName = Console.ReadLine();

            Console.Write("Enter quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            var newReservation = new ReservationModel(itemName, quantity);
            var (status, message) = _manager.AddReservation(newReservation);

            DisplayResult(status, message);
        }

        public void ViewReservations()
        {
            var (reservations, reservedQuantities) = _manager.GetReservations();

            if (reservations.Count > 0)
            {
                Console.WriteLine("Reservations:");
                foreach (var reservation in reservations)
                {
                    Console.WriteLine($"{reservation.ItemName}, Quantity: {reservation.Quantity}, Date: {reservation.ReservationDate}");
                }
            }
            else
            {
                Console.WriteLine("No reservations found.");
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Menu:");
                for (int i = 0; i < _menuItems.Length; i++)
                {
                    Console.WriteLine($"{i}. {_menuItems[i]}");
                }

                int choice = GetInput(_menuItems.Length - 1);

                switch (choice)
                {
                    case 0:
                        ViewInventory();
                        break;
                    case 1:
                        AddItem();
                        break;
                    case 2:
                        Search();
                        break;
                    case 3:
                        AddReservation();
                        break;
                    case 4:
                        ViewReservations();
                        break;
                    case 5:
                        return;
                }
            }
        }
    }
}