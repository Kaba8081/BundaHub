using Domain.Models;
using BundaHubManager.Services.Interfaces;

namespace BundaHubManager.Services
{
    public class BundaManager: IManager
    {
        private IList<ItemModel> _inventory = new List<ItemModel>(); 
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

        public IList<ItemModel> GetInventory()
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
            // - Check if item exists
            // - Check if quantity is available
            // - Check if the reservation is valid
            
            
            _reservations.Add(newReservation);
            return (true, "Reservation added successfully.");
        }

        public (bool, string) RemoveAt(int selection)
        {
            try
            {
                if (selection < 0 || selection >= _inventory.Length)
                {
                    return (false, "Invalid index.");
                }

                // Create a new array without the item at the specified index
                var newInventory = new ItemModel[_inventory.Length - 1];

                // Copy elements before the index
                Array.Copy(_inventory, 0, newInventory, 0, selection);

                // Copy elements after the index
                Array.Copy(_inventory, selection + 1, newInventory, selection, _inventory.Length - selection - 1);

                _inventory = newInventory;

                return (true, "Item removed successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error removing item: {ex.Message}");
            }
        }

    }
}
