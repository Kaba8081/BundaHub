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
        }

        public void ViewInventory()
        {
            Console.WriteLine("Viewing inventory...");
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