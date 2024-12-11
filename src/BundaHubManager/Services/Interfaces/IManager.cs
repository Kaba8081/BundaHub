using Domain.Models;

namespace BundaHubManager.Services.Interfaces
{
    public interface IManager
    {
        public List<ItemModel> GetInventory();
        (List<ReservationModel>, Dictionary<string, int>) GetReservations();
        (bool, string) AddItem(ItemModel newItem);
        (bool, string) AddReservation(ReservationModel newReservation);
        (bool, string) RemoveAt(int selection);
    }
}