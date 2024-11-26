using Domain;
using BundaHubManager.Services.Interfaces;

namespace BundaHubManager.Services
{
    public class BundaManager: IManager
    {
        private Item[] _inventory = new Item[] { }; 
        private List<Reservation> _reservations = new List<Reservation>();
        private Dictionary<string, int> _reservedQuantities = new Dictionary<string, int>();

        public BundaManager()
        {

            _inventory = new Item[]
            {
                new Item("Laptop", 1500, 10, false, false),
                new Item("Chair", 150, 200, false, false),
                new Item("Pen", 5, 3000, false, false),
                new Item("Mug", 25, 130, false, false)
            };

            _SortInventory("name", true);
        }
        
        private void _SortInventory(string sortBy, bool ascending = true)
        {
            switch (sortBy)
            {
                case "name":
                    
                    if (ascending) Array.Sort(_inventory, (x, y) => x.Name.CompareTo(y.Name));
                    else Array.Sort(_inventory, (x, y) => y.Name.CompareTo(x.Name));

                    break;
                case "price":
                    
                    if (ascending) Array.Sort(_inventory, (x, y) => x.Price.CompareTo(y.Price));
                    else Array.Sort(_inventory, (x, y) => y.Price.CompareTo(x.Price));

                    break;
                case "quantity":
                    
                    if (ascending) Array.Sort(_inventory, (x, y) => x.Quantity.CompareTo(y.Quantity));
                    else Array.Sort(_inventory, (x, y) => y.Quantity.CompareTo(x.Quantity));

                    break;
                default:
                    throw new ArgumentException("Invalid sortBy value. Allowed values are: name, price, quantity.");
            }
        }

        public Item[] GetInventory()
        {
            // TODO: Account for reserved quantities

            return _inventory;
        }

        public (bool, string) AddItem(Item newItem)
        {
            // TODO: Check if item already exists

            Array.Resize(ref _inventory, _inventory.Length + 1);
            _inventory[^1] = newItem;

            _SortInventory("name", true);

            return (true, "Item added successfully.");
        }

        public (List<Reservation>, Dictionary<string, int>) GetReservations()
        {
            return (_reservations, _reservedQuantities);
        }

        public (bool, string) AddReservation(Reservation newReservation)
        {
            // TODO: Check reservation validity
            // - Check if item exists
            // - Check if quantity is available
            // - Check if the reservation is valid
        
            _reservations.Add(newReservation);
            return (true, "Reservation added successfully.");
        }

    }
}
