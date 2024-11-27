using Domain.Models;
using BundaHubManager.Services.Interfaces;

namespace BundaHubManager.Services
{
    public class BundaManager: IManager
    {
        private ItemModel[] _inventory = new ItemModel[] { }; 
        private List<ReservationModel> _reservations = new List<ReservationModel>();
        private Dictionary<string, int> _reservedQuantities = new Dictionary<string, int>();

        public BundaManager()
        {

            _inventory = new ItemModel[]
            {
                new ItemModel("Laptop", 1500, 10, []),
                new ItemModel("Chair", 150, 200, []),
                new ItemModel("Pen", 5, 3000, []),
                new ItemModel("Mug", 25, 130, [])
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

        public ItemModel[] GetInventory()
        {
            // TODO: Account for reserved quantities

            return _inventory;
        }

        public (bool, string) AddItem(ItemModel newItem)
        {
            foreach (var item in _inventory)
            {
                if (item.Name.Equals(newItem.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return (false, "Item with the same name already exists in the inventory.");
                }
            }

            Array.Resize(ref _inventory, _inventory.Length + 1);
            _inventory[^1] = newItem;

            _SortInventory("name", true);

            return (true, "Item added successfully.");
        }

        public (List<ReservationModel>, Dictionary<string, int>) GetReservations()
        {
            return (_reservations, _reservedQuantities);
        }

        public (bool, string) AddReservation(ReservationModel newReservation)
        {
            // TODO: Check reservation validity
            var item = _inventory.FirstOrDefault(i => i.Name.Equals(newReservation.ItemName, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return (false, "Item does not exist in inventory.");
            }
            // - Check if quantity is available
            // - Check if the reservation is valid

            _reservations.Add(newReservation);
            return (true, "Reservation added successfully.");
        }

    }
}
