using Domain;
using BundaHubManager.Services.Interfaces;
using BundaHubManager.UI.Interfaces;

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

        private static void DisplaySearchResults(List<Item> results)
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
                inventory.Max(i => i.IsFragile.ToString().Length),
                inventory.Max(i => i.IsColdStored.ToString().Length),
            };

            
            if (colSizes[0] < "Inventory:".Length) colSizes[0] = "Inventory:".Length;
            if (colSizes[1] < "Price".Length) colSizes[1] = "Price".Length;
            if (colSizes[2] < "Quantity".Length) colSizes[2] = "Quantity".Length;
            if (colSizes[3] < "Total Price".Length) colSizes[3] = "Total Price".Length;
            if (colSizes[4] < "Fragile".Length) colSizes[4] = "Fragile".Length;
            if (colSizes[5] < "Cold Stored".Length) colSizes[5] = "Cold Stored".Length;

           
            Console.Write("Inventory:".PadLeft(colSizes[0]));
            Console.Write("Price".PadLeft(colSizes[1] + 2));
            Console.Write("Quantity".PadLeft(colSizes[2] + 2));
            Console.Write("Total Price".PadLeft(colSizes[3] + 2));
            Console.Write("Fragile".PadLeft(colSizes[4] + 2));
            Console.WriteLine("Cold Stored".PadLeft(colSizes[5] + 2));

            
            Console.WriteLine(new string('-', colSizes.Sum() + 10));

            
            foreach (var item in inventory)
            {
                Console.Write(item.Name.PadLeft(colSizes[0] + 2));
                Console.Write(item.Price.ToString().PadLeft(colSizes[1] + 2));
                Console.Write(item.Quantity.ToString().PadLeft(colSizes[2] + 2));
                Console.Write(item.TotalPrice.ToString().PadLeft(colSizes[3] + 2));
                Console.Write(item.IsFragile ? "Yes".PadLeft(colSizes[4] + 2) : "No".PadLeft(colSizes[4] + 2));
                Console.WriteLine(item.IsColdStored ? "Yes".PadLeft(colSizes[5] + 2) : "No".PadLeft(colSizes[5] + 2));
            }

            
            Console.WriteLine(new string('-', colSizes.Sum() + 10));
        }

        public void AddItem()
        {
            Console.WriteLine("Adding item...");
            Console.Write("Enter item name: ");
            string name = Console.ReadLine();

            Console.Write("Enter item price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid price. Item not added.");
                return;
            }

            Console.Write("Enter item quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid quantity. Item not added.");
                return;
            }

            bool isFragile;
            while (true)
            {
                Console.Write("Is the item fragile? (yes/no): ");
                string fragileInput = Console.ReadLine()?.Trim().ToLower();
                if (fragileInput == "yes")
                {
                    isFragile = true;
                    break;
                }
                else if (fragileInput == "no")
                {
                    isFragile = false;
                    break;
                }
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
            }

            bool isColdStored;
            while (true)
            {
                Console.Write("Should the item be cold stored? (yes/no): ");
                string coldStoredInput = Console.ReadLine()?.Trim().ToLower();
                if (coldStoredInput == "yes")
                {
                    isColdStored = true;
                    break;
                }
                else if (coldStoredInput == "no")
                {
                    isColdStored = false;
                    break;
                }
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
            }

            Item newItem = new Item(name, price, quantity, isFragile, isColdStored);
            var (status, message) = _manager.AddItem(newItem);
            DisplayResult(status, message);
        }

        public void Search()
        {
            var inventory = _manager.GetInventory();

            Console.WriteLine("Search by (name/price/quantity): ");
            string searchBy = Console.ReadLine()?.ToLower();

            switch (searchBy)
            {
                case "name":
                    Console.Write("Enter item name to search: ");
                    string name = Console.ReadLine();
                    var nameResults = inventory.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    DisplaySearchResults(nameResults);
                    break;

                case "price":
                    Console.Write("Enter minimum price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal minPrice))
                    {
                        Console.WriteLine("Invalid price.");
                        return;
                    }
                    Console.Write("Enter maximum price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
                    {
                        Console.WriteLine("Invalid price.");
                        return;
                    }
                    var priceResults = inventory.Where(i => i.Price >= minPrice && i.Price <= maxPrice).ToList();
                    DisplaySearchResults(priceResults);
                    break;

                case "quantity":
                    Console.Write("Enter minimum quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int minQuantity))
                    {
                        Console.WriteLine("Invalid quantity.");
                        return;
                    }
                    Console.Write("Enter maximum quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int maxQuantity))
                    {
                        Console.WriteLine("Invalid quantity.");
                        return;
                    }
                    var quantityResults = inventory.Where(i => i.Quantity >= minQuantity && i.Quantity <= maxQuantity).ToList();
                    DisplaySearchResults(quantityResults);
                    break;


                default:
                    Console.WriteLine("Invalid search criteria. Choose either 'name', 'price', or 'quantity'.");
                    break;
            }
        }

        public void AddReservation()
        {
            var inventory = _manager.GetInventory();
            var (reservations, reservedQuantities) = _manager.GetReservations();

            Console.Write("Enter item name to reserve: ");
            string itemName = Console.ReadLine();
            Console.Write("Enter quantity to reserve: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid quantity. Reservation not made.");
                return;
            }

            var item = inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                Console.WriteLine("Item not found.");
                return;
            }

            // Calculate total reserved quantity for the item
            int totalReservedQuantity = reservedQuantities.ContainsKey(itemName) ? reservedQuantities[itemName] : 0;

            // Calculate available quantity for reservation
            int availableForReservation = (int)item.Quantity - totalReservedQuantity;

            if (quantity > availableForReservation)
            {
                Console.WriteLine($"Only {availableForReservation} items available for reservation.");
                return;
            }

            // Update reserved quantities
            if (reservedQuantities.ContainsKey(itemName))
            {
                reservedQuantities[itemName] += quantity;
            }
            else
            {
                reservedQuantities[itemName] = quantity;
            }

            var (status, message) = _manager.AddReservation(new Reservation(itemName, quantity));
            DisplayResult(status, message);
        }

        public void ViewReservations()
        {
            var (reservations, _) = _manager.GetReservations();

            Console.WriteLine("Current Reservations:");
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Item: {reservation.ItemName}, Quantity: {reservation.Quantity}, Date: {reservation.ReservationDate}");
            }
        }

        public void Run() 
        {
            Console.WriteLine("Welcome to BundaHub!");
            while (true)
            {
                // Display the menu 
                for (int i = 0; i < _menuItems.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {_menuItems[i]}");
                }

                // Get user input
                int choice = GetInput(_menuItems.Length, 1);

                switch (choice)
                {
                    case 1:
                        ViewInventory();
                        break;
                    case 2:
                        AddItem();
                        break;
                    case 3:
                        Search();
                        break;
                    case 4:
                        AddReservation();
                        break;
                    case 5:
                        ViewReservations();
                        break;
                    case 6:
                        Console.WriteLine("Goodbye! - Thank you for using BundaHub.");
                        return;
                }
            }
        }
    }
}