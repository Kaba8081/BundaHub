using Domain.Models;
using BundaHubManager.Services.Interfaces;

namespace BundaHubManager.Services
{
    public class SectorManager: ISectorManager
    {
        IList<SectorModel> _sectors = new List<SectorModel> { };

        private IList<SectorModel> _filterSectors(IList<SectorModel> filteredSectors, string paramType, object filters) 
        {
            switch (paramType)
            {
                case "parameters":
                    filteredSectors = filteredSectors.Where(sector => sector.GetProperties.Contains(filters.ToString())).ToArray();
                    break;
            }

            return filteredSectors;
        }
        public IList<SectorModel> GetSectors(Dictionary<string, object>? parameters)
        {
            var filteredSectors = _sectors;

            if (parameters != null)
            {
                foreach (string paramType in parameters.Keys)
                {
                    filteredSectors = _filterSectors(filteredSectors, paramType, parameters[paramType]);
                }
            }

            return filteredSectors;
        }
        public SectorModel? GetSector(int sectorId)
        {
            return _sectors.FirstOrDefault(sector => sector.Id == sectorId);
        }

        public (bool, string) AddSector(SectorModel newSector)
        {
            // TODO: Validate newSector

            _sectors = _sectors.Append(newSector).ToArray();

            return (true, "Sector added successfully.");
        }
        public (bool, string) AddSubSector(int parentSectorId, int capacity)
        {
            var parentSector = GetSector(parentSectorId);
            if (parentSector == null) return (false, "Parent sector not found.");

            parentSector.AddSubSector(capacity);
            return (true, "Subsector added successfully.");
        }
        public (bool, string) AddSubSector(SectorModel parentSector, int capacity)
        {
            parentSector.AddSubSector(capacity);
            return (true, "Subsector added successfully.");
        }
        public (bool, string) UpdateSector(SectorModel updatedSector)
        {
            // TODO: Validate updatedSector

            var oldSector = GetSector(updatedSector.Id);

            if (oldSector == null) return (false, "Sector not found.");

            _sectors = _sectors.Select(sector => sector.Id == updatedSector.Id ? updatedSector : sector).ToArray();

            return (true, "Sector updated successfully.");
        }
        public (bool, string) DeleteSector(string sectorId)
        {
            var sector = GetSector(int.Parse(sectorId));

            if (sector == null) return (false, "Sector not found.");

            _sectors = _sectors.Where(sector => sector.Id != int.Parse(sectorId)).ToArray();

            return (true, "Sector deleted successfully.");
        }

        public ItemModel[] GetInventory(int? sectorId)
        {
            if (sectorId != null){
                var sector = GetSector(sectorId.Value);
                if (sector == null) return new ItemModel[] { };
                return sector.Inventory;
            }
            return _sectors.SelectMany(sector => sector.Inventory).ToArray();
        }
    }
}