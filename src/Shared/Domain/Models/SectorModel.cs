using Domain.Entites;

namespace Domain.Models
{
    public class SectorModel: Sector
    {
        public new SubSectorModel[] SubSectors { get; set; } = new SubSectorModel[] { };
        public new ItemModel[] Inventory { get; set; } = new ItemModel[] { };

        // TODO Add description to constructor
        public SectorModel(int id): this(id, "undefined") {}
        public SectorModel(int id, string name): this(id, name, 0) {}
        public SectorModel(int id, string name, int level): this(id, name, level, new ItemProperties[] { }) {}
        public SectorModel(int id, string name, int level, ItemProperties[] properties)
        {
            Id = id;
            Name = name;
            Level = level;
            Properties = properties;
        }
        public string Label { get => $"{Id} - {Name}"; }
        public int GetTotalCapacity { get => CalculateTotalCapacity(); }
        public string[] GetProperties { get => GetSectorProperties(); }
        public void AddSubSector(int capacity, char identifier = ' ')
        {
            if (identifier == ' ')
            {
                identifier = GetNextIdentifier();
            }
            SubSectors = SubSectors.Append(new SubSectorModel(this, identifier, capacity)).ToArray();
        }
        public char GetNextIdentifier()
        {
            char nextIdentifier = 'A';
            foreach (var subSector in SubSectors)
            {
                if (subSector.Identifier == nextIdentifier)
                {
                    nextIdentifier++;
                }
            }
            return nextIdentifier;
        }
        private int CalculateTotalCapacity()
        {
            int totalCapacity = 0;
            foreach (var subSector in SubSectors)
            {
                totalCapacity += subSector.Capacity;
            }
            return totalCapacity;
        }
        private string[] GetSectorProperties()
        {
            string[] properties = new string[Properties.Length];
            for (int i = 0; i < Properties.Length; i++)
            {
                properties[i] = Properties[i].ToString();
            }
            return properties;
        }
    }
    public class SubSectorModel: SubSector
    {
        public new ItemModel[] Inventory { get; set; } = new ItemModel[] { };
        public SubSectorModel (SectorModel parent, char identifier, int capacity) 
        {
            ParentId = parent.Id;
            Identifier = identifier;
            Capacity = capacity;
        }
        public string Label { get => $"{ParentId}{Identifier}"; }
    }
}
