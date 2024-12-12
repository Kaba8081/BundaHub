using Domain.Entites;

namespace Domain.Models
{
    public class ItemModel: Item
    {
        public ItemModel(string name) : this(name, 0, 1, [])
        {
        }
        public ItemModel(string name, decimal price, decimal v) : this(name, price, 1, [])
        {
        }
        public ItemModel(string name, decimal price, int quantity, ItemProperties[] properties)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            TotalPrice = price * quantity;
            Properties = properties;
        }
        public string[] GetProperties { get => GetItemProperties(); }
        private string[] GetItemProperties()
        {
            List<string> properties = new List<string>();
            for (int i = 0; i < Properties.Length; i++)
            {
                properties.Add(Properties[i].ToString());
            }
            return properties.ToArray();
        }
    }
}
