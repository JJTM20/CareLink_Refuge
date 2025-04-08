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
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Location { get; set; } 
        public int Capacity { get; set; } 
        public int CurrentOccupancy { get; set; } 

        public ICollection<Refugee> Refugees { get; set; }
    }
}