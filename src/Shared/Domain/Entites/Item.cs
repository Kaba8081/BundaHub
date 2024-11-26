
using Domain.Entites;

namespace Domain.Entites
{
    public class Item
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public ItemProperties[] Properties { get; set; }
    }
}
