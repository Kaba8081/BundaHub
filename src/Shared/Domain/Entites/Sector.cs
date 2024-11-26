namespace Domain.Entites
{
    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public SubSector[] SubSectors { get; set; }
        public Item[] Inventory { get; set; }
        public ItemProperties[] Properties { get; set; }

    }

    public class SubSector
    {  
        public int ParentId { get; set; }
        public char Identifier { get; set; }
        public int Capacity { get; set; }
        public int Level { get; set; }
        public Item[] Inventory { get; set; }
    }
}
