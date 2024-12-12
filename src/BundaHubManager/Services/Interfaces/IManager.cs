using Domain.Models;

namespace BundaHubManager.Services.Interfaces
{
    public interface IManager
    {
        public IList<ItemModel> GetInventory();
        IList<SectorModel> GetSectors(Dictionary<string, object>? parameters = null);
        IList<ReservationModel> GetReservations();
        (bool, string) AddItem(ItemModel newItem);
        (bool, string) AddItem(ItemModel newItem, int sectorId);
        (bool, string) AddItem(ItemModel newItem, int sectorId, int subSectorId);
        (bool, string) AddReservation(ReservationModel newReservation);
    }
}