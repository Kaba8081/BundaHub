﻿using Domain.Models;
using BundaHubManager.Services.Interfaces;
using Domain.Entites;

namespace BundaHubManager.Services
{
    public class BundaManager: IManager
    {
        private ISectorManager _sectorManager; 
        private IList<ReservationModel> _reservations = new List<ReservationModel>();
        private Dictionary<string, int> _reservedQuantities = new Dictionary<string, int>();
                private JsonDataHandler _jsonDataHandler;
        public BundaManager()
        {
            _sectorManager = new SectorManager();
            _jsonDataHandler = new JsonDataHandler();

            // Initialize sectors and items
            InitializeSectors();
        }
        private void InitializeSectors()
        {
            //Deafult data
            //Delete before project presentation


            /*_sectorManager.AddSector(new SectorModel(1, "Main storage", 1));
            _sectorManager.AddSubSector(1, 1000);
            _sectorManager.AddSubSector(1, 2000);
            _sectorManager.AddSubSector(1, 500);

            _sectorManager.GetSectors().First().SubSectors.First().Inventory = new ItemModel[]
            {
                new ItemModel("Laptop", 1500, 10, []),
                new ItemModel("Chair", 150, 200, []),
                new ItemModel("Pen", 5, 3000, []),
                new ItemModel("Mug", 25, 130, [ItemProperties.FRAGILE])
            };

            _sectorManager.AddSector(new SectorModel(2, "Food storage", 1, [ItemProperties.FREEZER]));
            _sectorManager.AddSubSector(2, 5000);
            _sectorManager.AddSubSector(2, 250);
            _sectorManager.GetSector(2).SubSectors.First().Inventory = new ItemModel[]
            {
                new ItemModel("Apple", 25, 100, [ItemProperties.FREEZER]),
                new ItemModel("Banana", 50, 200, [ItemProperties.FREEZER]),
                new ItemModel("Orange", 100, 150, [ItemProperties.FREEZER]),
                new ItemModel("Pineapple", 10, 50, [ItemProperties.FREEZER])
            };

            _sectorManager.AddSector(new SectorModel(3, "Electronics storage", 2));
            _sectorManager.AddSubSector(3, 6000);
            */

            LoadDataFromJson();
        }
        private void LoadDataFromJson()
        {
            // Load sectors from JSON file
            var sectors = _jsonDataHandler.LoadData<List<SectorModel>>("sectors.json");
            foreach (var sector in sectors)
            {
                _sectorManager.AddSector(sector);
            }

            // Load reservations from JSON file
            _reservations = _jsonDataHandler.LoadData<List<ReservationModel>>("reservations.json");
        }
        private void SaveDataToJson()
        {
            var sectors = _sectorManager.GetSectors();
            _jsonDataHandler.SaveData("sectors.json", sectors);

            var reservations = GetReservations();
            _jsonDataHandler.SaveData("reservations.json", reservations);
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
        private static IList<ItemModel> _CalculateAvailableQuantities(IList<ItemModel> inventory, IList<ReservationModel> reservations) 
        {
            var resultInventory = inventory;

            foreach (var rsrv in reservations) 
            {
                if (resultInventory.Any(x => x.Name.Equals(rsrv.ItemName, StringComparison.OrdinalIgnoreCase)))
                {
                    var existingItem = resultInventory.First(x => x.Name.Equals(rsrv.ItemName, StringComparison.OrdinalIgnoreCase));
                    existingItem.Quantity -= rsrv.Quantity;

                    // Ensure the quantity is not negative
                    // * Shouldn't happend, but making reservations doesn't have a check for quantity
                    if (existingItem.Quantity < 0) existingItem.Quantity = 0;
                }
            }

            return resultInventory;
        }
        private IList<ItemModel> _SortInventory(string sortBy, bool ascending = true)
        {
            var _inventory = GetInventory();

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

            return _inventory;
        }
        public IList<ItemModel> GetInventory()
        {
            // TODO: Add a parameter for specific sorting

            IList<IList<ItemModel>> _sector_inventories = new List<IList<ItemModel>>();
            IList<ReservationModel> _reservations = GetReservations();
            IList<ItemModel> _fullInventory = new List<ItemModel>();

            foreach (var sector in _sectorManager.GetSectors())
            {
                _sector_inventories.Add(sector.Inventory);
            }
            
            _fullInventory = _MergeInventories(_sector_inventories);
            _fullInventory = _CalculateAvailableQuantities(_fullInventory, _reservations);

            return _fullInventory;
        }
        public IList<SectorModel> GetSectors(Dictionary<string, object>? parameters)
        {
            if (parameters == null) return _sectorManager.GetSectors();
            return _sectorManager.GetSectors(parameters);
        }
        public (bool, string) AddItem(ItemModel newItem)
        {
            var _subSectors = _sectorManager.GetSectors().SelectMany(x => x.SubSectors).ToList();
            var _properties = newItem.Properties;

            // Can be done better

            // Filter subsectors by properties
            _subSectors = _subSectors.Where(x => _properties.All(p => _sectorManager.GetSector(x.ParentId)?.Properties.Contains(p) == true)).ToList();
            // Filter subsectors by capacity
            _subSectors = _subSectors.Where(x => x.Capacity >= newItem.Quantity).ToList();
            // Sort subsectors by capacity
            _subSectors = _subSectors.OrderBy(x => x.Capacity).ToList();

            if (_subSectors.Any())
            {
                _subSectors.First().AddItem(newItem);
                SaveDataToJson();
                return (true, "Item added successfully.");
            }

            return (false, "No available space in any sector.");
        }
        public (bool, string) AddItem(ItemModel newItem, int sectorId)
        {
            throw new NotImplementedException();

            return (false, "No available space in any sector.");
        }
        public (bool, string) AddItem(ItemModel newItem, int sectorId, int subSectorId)
        {
            throw new NotImplementedException();
            
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
            SaveDataToJson();
            return (true, "Reservation added successfully.");
        }
        public (bool, string) RemoveItem(ItemModel item)
        {
            
            var sectors = _sectorManager.GetSectors();

            foreach (var sector in sectors)
            {
                foreach (var subSector in sector.SubSectors)
                {
                    
                    var existingItem = subSector.Inventory.FirstOrDefault(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                    if (existingItem != null)
                    {
                        
                        subSector.Inventory = subSector.Inventory.Where(x => x.Name != existingItem.Name).ToArray();
                        SaveDataToJson();
                        return (true, "Item removed successfully.");
                    }
                }
            }

            return (false, "Item not found.");
        }
    }
}
