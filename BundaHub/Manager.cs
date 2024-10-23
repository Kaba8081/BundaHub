namespace BundaHub
{
    class BundaHubManager
    {
        private Item[] _inventory = new Item[]{}; // This is the inventory of the store

        public BundaHubManager()
        {
            // Add some items to the inventory
            _inventory = new Item[]
            {
                new Item("Laptop", 1500, 10),
                new Item("Chair", 150, 200),
                new Item("Pen", 5, 3000),
                new Item("Mug", 25, 130)
            };

            _SortInventory("name", true);
        }
        private void _SortInventory(string sortBy, bool ascending = true)
        {
            switch (sortBy)
            {
            case "name":
                // Sort the inventory by name
                if (ascending) Array.Sort(_inventory, (x, y) => x.Name.CompareTo(y.Name));
                else Array.Sort(_inventory, (x, y) => y.Name.CompareTo(x.Name));
                
                break;
            case "price":
                // Sort the inventory by price
                if (ascending) Array.Sort(_inventory, (x, y) => x.Price.CompareTo(y.Price));
                else Array.Sort(_inventory, (x, y) => y.Price.CompareTo(x.Price));

                break;
            case "quantity":
                // Sort the inventory by quantity
                if (ascending) Array.Sort(_inventory, (x, y) => x.Quantity.CompareTo(y.Quantity));
                else Array.Sort(_inventory, (x, y) => y.Quantity.CompareTo(x.Quantity));

                break;
            default:
                throw new ArgumentException("Invalid sortBy value. Allowed values are: name, price, quantity.");
            }
        }

        public void ViewInventory()
        {
            // Calculate the column sizes based on the biggest item in each column
            int[] colSizes = {
                _inventory.Max(i => i.Name.Length),
                _inventory.Max(i => i.Price.ToString().Length),
                _inventory.Max(i => i.Quantity.ToString().Length),
                _inventory.Max(i => i.TotalPrice.ToString().Length)
            };

            // the colSize could be smaller than the column header
            if (colSizes[0] < "Inventory:".Length) colSizes[0] = "Inventory:".Length;
            if (colSizes[1] < "Price".Length) colSizes[1] = "Price".Length;
            if (colSizes[2] < "Quantity".Length) colSizes[2] = "Quantity".Length;
            if (colSizes[3] < "Total Price".Length) colSizes[3] = "Total Price".Length;

            // header row
            Console.Write("Inventory:".PadLeft(colSizes[0]));
            Console.Write("Price".PadLeft(colSizes[1] + 2));
            Console.Write("Quantity".PadLeft(colSizes[2] + 2));
            Console.WriteLine("Total Price".PadLeft(colSizes[3] + 2));

            // separator line
            Console.WriteLine(new string('-', colSizes.Sum() + 10));
            
            // each item row
            foreach (Item item in _inventory)
            {
                Console.Write(item.Name.PadLeft(colSizes[0] + 2));
                Console.Write(item.Price.ToString().PadLeft(colSizes[1] + 2));
                Console.Write(item.Quantity.ToString().PadLeft(colSizes[2] + 2));
                Console.WriteLine(item.TotalPrice.ToString().PadLeft(colSizes[3] + 2));
            }

            // separator line
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

                            Item newItem = new Item(name, price, quantity);
                            Array.Resize(ref _inventory, _inventory.Length + 1);
                            _inventory[^1] = newItem;

                            Console.WriteLine("Item added successfully.");
                            _SortInventory("name", true);
        }

        public void Search()
        {
               Console.WriteLine("Search by (name/price/quantity): ");
    string searchBy = Console.ReadLine()?.ToLower(); 

    switch (searchBy)
    {
        case "name":
            Console.Write("Enter item name to search: ");
            string name = Console.ReadLine();
            var nameResults = _inventory.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
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
            var priceResults = _inventory.Where(i => i.Price >= minPrice && i.Price <= maxPrice).ToList();
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
            var quantityResults = _inventory.Where(i => i.Quantity >= minQuantity && i.Quantity <= maxQuantity).ToList();
            DisplaySearchResults(quantityResults);
            break;


        default:
            Console.WriteLine("Invalid search criteria. Choose either 'name', 'price', or 'quantity'.");
            break;

            
    }
    
}
        private void DisplaySearchResults(List<Item> results)
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

        }
    }
