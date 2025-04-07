using Microsoft.EntityFrameworkCore;

namespace CareLink_Refugee.Models
{
    public enum Gender
    {
        Male = 0,
        Female = 1,
        NonBinary = 2,
    }
    public class Refugee
    {
        // Empty constructor is required for EF Core to work properly:)
        public Refugee() {
            LanguagesSpoken = new List<string>();
            MedicalConditions = new List<string>();
        }
        public Refugee(Guid id, string firstName, string lastName, DateTime dateOfBirth, string gender, string nationality, 
            Shelter accomodation, DateTime dateOfArrival, string status,
            ICollection<string> languagesSpoken, ICollection<string> medicalConditions, DateTime createdAt, DateTime updatedAt, int familyId)
        {
            Id=id;
            FirstName=firstName;
            LastName=lastName;
            DateOfBirth=dateOfBirth;
            Gender=gender;
            Nationality=nationality;
            Accomodation=accomodation;
            DateOfArrival=dateOfArrival;
            Status=status;
            LanguagesSpoken=languagesSpoken;
            MedicalConditions=medicalConditions;
            CreatedAt=createdAt;
            UpdatedAt=updatedAt;
            FamilyId=familyId;
        }

        public Guid Id { get; set; } // Unique identifier
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } // Could also be an enum
        public string Nationality { get; set; }
        public Guid AccomodationId { get; set; } // foreign key for the shelter
        public Shelter Accomodation { get; set; } // e.g., camp name, city, or address
        public DateTime DateOfArrival { get; set; } // When they arrived at the current location
        public string Status { get; set; }
        public ICollection<string> LanguagesSpoken { get; set; }
        public ICollection<string> MedicalConditions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int FamilyId { get; set; }
    }

}
