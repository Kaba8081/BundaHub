using Domain.Models;

namespace BundaHubManager.Services.Interfaces
{
    public interface IManager
    {
        public IList<ItemModel> GetInventory();
        IList<SectorModel> GetSectors(Dictionary<string, object>? parameters = null);
        IList<ReservationModel> GetReservations();
        (bool, string) AddItem(ItemModel newItem);
        (bool, string) AddReservation(ReservationModel newReservation);
    }
}