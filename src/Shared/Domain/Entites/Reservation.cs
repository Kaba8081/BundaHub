namespace Domain
{
    public class Reservation
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime ReservationDate { get; set; }

        public Reservation(string itemName, int quantity)
        {
            ItemName = itemName;
            Quantity = quantity;
            ReservationDate = DateTime.Now;
        }
    }
} 