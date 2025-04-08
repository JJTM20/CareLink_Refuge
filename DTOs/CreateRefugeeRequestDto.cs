
using System.ComponentModel.DataAnnotations;

namespace CareLink_Refugee.DTOs
{
    public class CreateRefugeeRequestDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        public Guid AccomodationId { get; set; }

        [Required]
        public DateTime DateOfArrival { get; set; }

        [Required]
        public string Status { get; set; }

        public ICollection<string> LanguagesSpoken { get; set; } = new List<string>();

        public ICollection<string> MedicalConditions { get; set; } = new List<string>();

        public Guid? FamilyId { get; set; }
    }
}

