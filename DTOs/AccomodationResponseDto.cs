using CareLink_Refugee.Models;

namespace CareLink_Refugee.DTOs
{
    public class AccomodationResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
    }
}
