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
            Console.WriteLine("Viewing inventory...");
            foreach (Item item in _inventory)
            {
                Console.WriteLine($"Name: {item.Name}, Price: {item.Price}, Quantity: {item.Quantity}, Total Price: {item.TotalPrice}");
            }
        }

        public void AddItem()
        {
            Console.WriteLine("Adding item...");
        }

        public void Search()
        {
            Console.WriteLine("Searching...");
        }
    }
}