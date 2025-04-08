using CareLink_Refugee.Models;

namespace CareLink_Refugee.DTOs
{
    public class FamilyResponseDto
    {
        public Guid Id { get; set; }
        public string FamilyName { get; set; }
        public ICollection<CreateRefugeeResponseDto> Members { get; set; }
    }
}
