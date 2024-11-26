using Domain.Models;
using BundaHubManager.Services.Interfaces;

using System.Linq;

namespace BundaHubManager.Services
{
    public class SectorManager: ISectorManager
    {
        SectorModel[] _sectors = new SectorModel[] { };

        public SectorModel[] GetSectors(string[]? parameters)
        {
            // TODO Implement filtering based on provided parameters

            return _sectors;
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
    }
}