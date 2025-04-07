namespace CareLink_Refugee.Models
{
    public class Shelter
    {
        public Shelter() {
            Refugees = new List<Refugee>();
        }
        public Shelter(Guid id, string name, string location, int capacity, int currentOccupancy)
        {
            Id = id;
            Name = name;
            Location = location;
            Capacity = capacity;
            CurrentOccupancy = currentOccupancy;
        }
        public Guid Id { get; set; } // Unique identifier
        public string Name { get; set; } // Name of the shelter
        public string Location { get; set; } // Location of the shelter
        public int Capacity { get; set; } // Maximum capacity of the shelter
        public int CurrentOccupancy { get; set; } // Current number of refugees in the shelter

        public ICollection<Refugee> Refugees { get; set; }
    }
}