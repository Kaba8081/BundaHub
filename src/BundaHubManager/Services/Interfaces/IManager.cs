using Domain.Models;

namespace BundaHubManager.Services.Interfaces
{
    public interface IManager
    {
        ItemModel[] GetInventory();
        IList<SectorModel> GetSectors(Dictionary<string, object>? parameters = null);
        List<ReservationModel> GetReservations();
        (bool, string) AddItem(ItemModel newItem);
        (bool, string) AddReservation(ReservationModel newReservation);
    }
}