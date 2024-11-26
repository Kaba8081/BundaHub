using Domain;

namespace BundaHubManager.Services.Interfaces
{
    public interface IManager
    {
        Item[] GetInventory();
        (List<Reservation>, Dictionary<string, int>) GetReservations();
        (bool, string) AddItem(Item newItem);
        (bool, string) AddReservation(Reservation newReservation);
    }
}