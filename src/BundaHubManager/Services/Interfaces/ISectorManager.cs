using Domain.Models;

namespace BundaHubManager.Services.Interfaces
{
    public interface ISectorManager
    {
        SectorModel[] GetSectors(string[]? parameters = null);
        SectorModel? GetSector(int sectorId);
        (bool, string) AddSector(SectorModel newSector);
        (bool, string) AddSubSector(int parentSectorId, int capacity);
        (bool, string) AddSubSector(SectorModel parentSector, int capacity);
        (bool, string) UpdateSector(SectorModel updatedSector);
        (bool, string) DeleteSector(string sectorId);
    }
}