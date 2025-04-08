namespace CareLink_Refugee.Models
{
    public class Family
    {
        public Family()
        {
            Members = new List<Refugee>();
        }
        public Family(Guid id, string familyName)
        {
            Id=id;
            FamilyName=familyName;
            Members = new List<Refugee>();
        }

        public Guid Id { get; set; }
        public string FamilyName { get; set; }

        public ICollection<Refugee> Members { get; set; }
    }
}
