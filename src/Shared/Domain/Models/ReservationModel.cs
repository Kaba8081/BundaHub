using Domain.Entites;

namespace Domain.Models
{
    public class ReservationModel: Reservation
    {
        public ReservationModel(string itemName, int quantity)
        {
            ItemName = itemName;
            Quantity = quantity;
            ReservationDate = DateTime.Now;
        }
    }
}
