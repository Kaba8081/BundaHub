using Domain.Models;

namespace BundaHubManager.Services.Interfaces
{
    public interface IManager
    {
        public List<ItemModel> GetInventory();
        IList<SectorModel> GetSectors(Dictionary<string, object>? parameters = null);
        List<ReservationModel> GetReservations();
        (bool, string) AddItem(ItemModel newItem);
        (bool, string) AddReservation(ReservationModel newReservation);
        (bool, string) RemoveAt(int selection);
    }
}