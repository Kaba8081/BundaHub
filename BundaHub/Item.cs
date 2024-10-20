namespace BundaHub
{
    class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }

        public Item(string name) : this(name, 0, 1)
        {
        }
        public Item(string name, int price) : this(name, price, 1)
        {   
        }
        public Item(string name, int price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            TotalPrice = price * quantity;
        }
    }
}