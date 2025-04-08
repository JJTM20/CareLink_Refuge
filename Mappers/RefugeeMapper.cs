using CareLink_Refugee.Models;
using CareLink_Refugee.DTOs;

namespace CareLink_Refugee.Mappers
{
    public static class RefugeeMapper
    {
        public static Refugee ToModel(this CreateRefugeeRequestDto dto)
        {
            return new Refugee
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Nationality = dto.Nationality,
                AccomodationId = dto.AccomodationId,
                DateOfArrival = dto.DateOfArrival,
                Status = dto.Status,
                LanguagesSpoken = dto.LanguagesSpoken,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                MedicalConditions = dto.MedicalConditions,
                FamilyId = dto.FamilyId
            };
        }
        public static Refugee ToModel(this UpdateRefugeeRequestDto dto, Refugee existingRefugee)
        {
            existingRefugee.FirstName = dto.FirstName ?? existingRefugee.FirstName;
            existingRefugee.LastName = dto.LastName ?? existingRefugee.LastName;
            existingRefugee.DateOfBirth = dto.DateOfBirth ?? existingRefugee.DateOfBirth;
            existingRefugee.AccomodationId = dto.AccomodationId == null ? existingRefugee.AccomodationId : dto.AccomodationId;
            existingRefugee.DateOfArrival = dto.DateOfArrival ?? existingRefugee.DateOfArrival;
            existingRefugee.Status = dto.Status ?? existingRefugee.Status;
            existingRefugee.LanguagesSpoken = dto.LanguagesSpoken?.Any() == true ? dto.LanguagesSpoken : existingRefugee.LanguagesSpoken;
            existingRefugee.MedicalConditions = dto.MedicalConditions?.Any() == true ? dto.MedicalConditions : existingRefugee.MedicalConditions;
            existingRefugee.FamilyId = dto.FamilyId == null ? existingRefugee.FamilyId : dto.FamilyId;
            existingRefugee.UpdatedAt = DateTime.UtcNow;

            return existingRefugee;
        }
        public static CreateRefugeeResponseDto ToDto(this Refugee refugee)
        {
            return new CreateRefugeeResponseDto
            {
                Id = refugee.Id,
                FirstName = refugee.FirstName,
                LastName = refugee.LastName,
                DateOfBirth = refugee.DateOfBirth,
                Gender = refugee.Gender,
                Nationality = refugee.Nationality,
                AccomodationId = refugee.AccomodationId,
                DateOfArrival = refugee.DateOfArrival,
                Status = refugee.Status,
                LanguagesSpoken = refugee.LanguagesSpoken,
                MedicalConditions = refugee.MedicalConditions,
                CreatedAt = refugee.CreatedAt,
                UpdatedAt = refugee.UpdatedAt,
                FamilyId = refugee.FamilyId
            };
        }
        public static FamilyResponseDto ToDto(this Family family)
        {
            return new FamilyResponseDto
            {
                Id = family.Id,
                FamilyName = family.FamilyName,
                Members = family.Members.Select(m => m.ToDto()).ToList(),
            };
        }
        public static AccomodationResponseDto ToDto(this Shelter accomodation)
        {
            return new AccomodationResponseDto
            {
                Id = accomodation.Id,
                Name = accomodation.Name,
                Location = accomodation.Location,
                Capacity = accomodation.Capacity,
                CurrentOccupancy = accomodation.CurrentOccupancy,
                Refugees = accomodation.Refugees.Select(r => r.ToDto()).ToList(),
            };
        }
        public static Shelter ToModel(this CreateAccomodationRequestDto dto)
        {
            return new Shelter
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Location = dto.Location,
                Capacity = dto.Capacity,
                CurrentOccupancy = dto.CurrentOccupancy
            };
        }
    }
}
