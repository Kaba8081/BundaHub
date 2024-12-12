using Domain.Models;
using BundaHubManager.Services.Interfaces;

namespace BundaHubManager.Services
{
    public class BundaManager: IManager
    {
        private ISectorManager _sectorManager;
        private IList<ItemModel> _inventory = new List<ItemModel>(); 
        private IList<ReservationModel> _reservations = new List<ReservationModel>();
        private Dictionary<string, int> _reservedQuantities = new Dictionary<string, int>();

        public BundaManager()
        {
            _sectorManager = new SectorManager();
            _sectorManager.AddSector(new SectorModel(1, "Food storage", 100));
            _sectorManager.AddSubSector(1, 50);
            _sectorManager.AddSubSector(1, 50);

            _sectorManager.AddSector(new SectorModel(2, "Electronics storage", 300));
            _sectorManager.AddSubSector(2, 50);
            _sectorManager.AddSubSector(2, 50);


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
                if (ascending) _inventory = _inventory.OrderBy(x => x.Name).ToList();
                else _inventory = _inventory.OrderByDescending(x => x.Name).ToList();
                break;
            case "price":
                if (ascending) _inventory = _inventory.OrderBy(x => x.Price).ToList();
                else _inventory = _inventory.OrderByDescending(x => x.Price).ToList();
                break;
            case "quantity":
                if (ascending) _inventory = _inventory.OrderBy(x => x.Quantity).ToList();
                else _inventory = _inventory.OrderByDescending(x => x.Quantity).ToList();
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
        public IList<SectorModel> GetSectors(Dictionary<string, object>? parameters)
        {
            if (parameters == null) return _sectorManager.GetSectors();
            return _sectorManager.GetSectors(parameters);
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

            _inventory.Add(newItem);

            _SortInventory("name", true);

            return (true, "Item added successfully.");
        }
        public IList<ReservationModel> GetReservations()
        {
            // TODO: Filter only reservations that are not expired
            return _reservations;
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

    }
}
