using Domain.Models;
using BundaHubManager.Services.Interfaces;

namespace BundaHubManager.Services
{
    public class BundaManager: IManager
    {
        private ISectorManager _sectorManager; 
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


           _sectorManager.GetSectors().First().Inventory = new ItemModel[]
            {
                new ItemModel("Laptop", 1500, 10, []),
                new ItemModel("Chair", 150, 200, []),
                new ItemModel("Pen", 5, 3000, []),
                new ItemModel("Mug", 25, 130, [])
            };

            _SortInventory("name", true);
        }
        
        private static IList<ItemModel> _MergeInventories(IList<IList<ItemModel>> inventories)
        {
            IList<ItemModel> fullInventory = new List<ItemModel>();

            foreach (var inventory in inventories)
            {
                foreach (var item in inventory)
                {
                    // if the item already exists in the full inventory, add the quantities
                    if (fullInventory.Any(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        var existingItem = fullInventory.First(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                        existingItem.Quantity += item.Quantity;
                    }
                    // else add the new item to the list
                    else fullInventory.Add(item);
                }
            }
            return fullInventory;
        }
        private void _SortInventory(string sortBy, bool ascending = true)
        {
            var _inventory = this.GetInventory();
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

            IList<IList<ItemModel>> _sector_inventories = new List<IList<ItemModel>>();

            foreach (var sector in _sectorManager.GetSectors())
            {
                foreach (var subSector in sector.SubSectors)
                {
                    _sector_inventories.Add(subSector.Inventory);
                }
            }
            var newInv = _MergeInventories(_sector_inventories);
            return newInv;
        }
        public IList<SectorModel> GetSectors(Dictionary<string, object>? parameters)
        {
            if (parameters == null) return _sectorManager.GetSectors();
            return _sectorManager.GetSectors(parameters);
        }
        public (bool, string) AddItem(ItemModel newItem, int? sectorId=null, int? subSectorId = null)
        {
            // TODO: Implement sector and subsector filtering

            var _subSectors = _sectorManager.GetSectors().SelectMany(x => x.SubSectors).ToList();
            // Add the item to any sector/subsector if no sectorId is provided
            var existingSubSector = _subSectors.FirstOrDefault(
                subSector => 
                    subSector.Inventory.Any(
                        item => item.Name.Equals(
                            newItem.Name, StringComparison.OrdinalIgnoreCase)
                    ) 
                    && subSector.Capacity >= subSector.Inventory.Length + newItem.Quantity
            );
            // if the item already extists in the inventory, add the quantity
            if (existingSubSector != null)
            {
                existingSubSector.AddItem(newItem);
                return (true, "Item added successfully.");
            }

            // otherwise find any subsector with available space
            foreach (var subSector in _subSectors)
            {
                if (subSector.Capacity >= subSector.Inventory.Length + 1)
                {
                    subSector.AddItem(newItem);
                    return (true, "Item added successfully.");
                }
            }

            return (false, "No available space in any sector.");
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
