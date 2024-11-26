using Domain.Entites;

namespace Domain.Models
{
    internal class SectorModel: Sector
    {
        public string Label { get => $"{Id} - {Name}"; }
        public int GetTotalCapacity { get => CalculateTotalCapacity(); }
        public string[] GetProperties { get => GetSectorProperties(); }
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
        public string Label { get => $"{ParentId}{Identifier}"; }
    }
}
