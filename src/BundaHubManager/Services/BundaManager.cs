using Domain.Models;
using BundaHubManager.Services.Interfaces;
using Domain.Services;

namespace BundaHubManager.Services
{
    public class BundaManager : IManager
    {
        private ItemModel[] _inventory;
        private List<ReservationModel> _reservations;
        private Dictionary<string, int> _reservedQuantities;

        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string ProjectDirectory = Path.GetFullPath(Path.Combine(BaseDirectory, @"..\..\..\..\.."));
        private static readonly string ItemsFilePath = Path.Combine(ProjectDirectory, "src", "BundaHubManager", "Data", "items.json");
        private static readonly string ReservedItemsFilePath = Path.Combine(ProjectDirectory, "src", "BundaHubManager", "Data", "reservedItems.json");

        public BundaManager()
        {
            _inventory = JsonHandler.LoadFromFile<ItemModel[]>(ItemsFilePath) ?? new ItemModel[] { };
            _reservations = JsonHandler.LoadFromFile<List<ReservationModel>>(ReservedItemsFilePath) ?? new List<ReservationModel>();
            _reservedQuantities = new Dictionary<string, int>();

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
            return _inventory;
        }

        public (bool, string) AddItem(ItemModel newItem)
        {
            // TODO: Check if item already exists

            Array.Resize(ref _inventory, _inventory.Length + 1);
            _inventory[^1] = newItem;

            _SortInventory("name", true);
            JsonHandler.SaveToFile(ItemsFilePath, _inventory);

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
            JsonHandler.SaveToFile(ReservedItemsFilePath, _reservations);

            return (true, "Reservation added successfully.");
        }
    }
}
