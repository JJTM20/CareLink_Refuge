using CareLink_Refugee.Models;
using System.ComponentModel.DataAnnotations;

namespace CareLink_Refugee.DTOs
{
    public class CreateFamilyRequestDto
    {
        [Required]
        public string FamilyName { get; set; }

        [Required]
        public ICollection<Refugee> Members { get; set; }
    }
}
