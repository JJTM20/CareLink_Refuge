using CareLink_Refugee.Models;

namespace CareLink_Refugee.DTOs
{
    public class AccomodationResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
        public ICollection<CreateRefugeeResponseDto> Refugees { get; set; }
    }
}
