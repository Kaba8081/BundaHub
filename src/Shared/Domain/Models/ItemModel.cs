using Domain.Entites;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class ItemModel : Item
    {
        [JsonConstructor]
        public ItemModel(string name, decimal price, int quantity, ItemProperties[] properties)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            TotalPrice = price * quantity;
            Properties = properties;
        }

        public ItemModel(string name) : this(name, 0, 1, Array.Empty<ItemProperties>())
        {
        }

        public ItemModel(string name, decimal price) : this(name, price, 1, Array.Empty<ItemProperties>())
        {
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
