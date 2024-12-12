namespace Domain.Entites
{
    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; }= "No name provided";
        public string Description { get; set; } = "No description provided";
        public int Level { get; set; }
        public SubSector[] SubSectors { get; set; } = new SubSector[] { };
        public Item[] Inventory { get; set; } = new Item[] { };
        public ItemProperties[] Properties { get; set;  } = new ItemProperties[] { };

    }

    public class SubSector
    {  
        public int ParentId { get; set; }
        public char Identifier { get; set; }
        public int Capacity { get; set; }
        public Item[] Inventory { get; set; } = new Item[] { };
    }
}
