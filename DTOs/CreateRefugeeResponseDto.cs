using System.ComponentModel.DataAnnotations;

namespace CareLink_Refugee.DTOs
{
    public class CreateRefugeeResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Nationality { get; set; }

        public Guid? AccomodationId { get; set; }

        public DateTime DateOfArrival { get; set; }

        public string Status { get; set; }

        public ICollection<string> LanguagesSpoken { get; set; } = new List<string>();

        public ICollection<string> MedicalConditions { get; set; } = new List<string>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid? FamilyId { get; set; }
    }
}
