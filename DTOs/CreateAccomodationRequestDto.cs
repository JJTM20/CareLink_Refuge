namespace CareLink_Refugee.DTOs
{
    public class CreateAccomodationRequestDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
    }
}