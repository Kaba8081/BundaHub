
namespace Domain
{
    public class Item
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsFragile { get; set; }
        public bool IsColdStored { get; set; }

        public Item(string name) : this(name, 0, 1, false, false)
        {
        }
        public Item(string name, decimal price) : this(name, price, 1, false, false)
        {
        }
        public Item(string name, decimal price, int quantity, bool isfragile, bool iscoldstored)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            TotalPrice = price * quantity;
            IsFragile = isfragile;
            IsColdStored = iscoldstored;
        }
    }
}
