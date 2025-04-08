using CareLink_Refugee.Models;
using Bogus;

namespace CareLink_Refugee.Utils
{
    public static class DataGenerator
    {
        public static List<Refugee> Refugees = new();
        public static List<Shelter> Shelters = new();
        public static List<Family> Families = new();

        public static void GenerateMockData()
        {
            var shelterNames = new[]
                {
                    "Oksbøl",
                    "Kløvermarken",
                    "Dragsbæklejren",
                    "Sandholm",
                    "Avnstrup",
                    "Sjælsmark",
                    "Center Ellebæk",
                    "Kærshovedgård",
                    "Vesthimmerland",
                    "Jelling",
                    "Thisted",
                    "Brovst",
                    "Sigerslev",
                    "Gribskov",
                    "Børnecenter Tullebølle",
                    "Bornholm Center",
                    "Hvidovre Transit Center"
                };

            var shelterFaker = new Faker<Shelter>()
                .RuleFor(s => s.Id, f => f.Random.Guid())
                .RuleFor(s => s.Name, f => f.PickRandom<string>(shelterNames))
                .RuleFor(s => s.Location, f => f.Address.City())
                .RuleFor(s => s.Capacity, f => f.Random.Int(50, 500))
                .RuleFor(s => s.CurrentOccupancy, f => f.Random.Int(0, 500));

            var shelters = shelterFaker.Generate(shelterNames.Length);
            Shelters.AddRange(shelters);

            var familyFaker = new Faker<Family>()
                .RuleFor(f => f.Id, f => f.Random.Guid())
                .RuleFor(f => f.FamilyName, f => f.Name.LastName())
                .RuleFor(f => f.Members, f => new List<Refugee>());

            var families = familyFaker.Generate(30);
            Families.AddRange(families);

            var refugeeFaker = new Faker<Refugee>()
                .RuleFor(r => r.Id, f => f.Random.Guid())
                .RuleFor(r => r.FirstName, f => f.Name.FirstName())
                .RuleFor(r => r.LastName, f => f.Name.LastName())
                .RuleFor(r => r.Accomodation, f => f.PickRandom(Shelters))
                .RuleFor(r => r.DateOfBirth, f => f.Date.Past(50,DateTime.UtcNow))
                .RuleFor(r => r.Gender, f => f.Person.Gender.ToString())
                .RuleFor(r => r.Nationality, f => f.Address.Country())
                .RuleFor(r => r.DateOfArrival, f => f.Date.Past(5, DateTime.UtcNow))
                .RuleFor(r => r.Status, f => f.PickRandom(new[] { "Approved", "Transferred", "Resettled", "Pending" }))
                .RuleFor(r => r.LanguagesSpoken, f => f.PickRandom(new[] {"English", "Arabic", "Ukrainian", "Wutana", "Shabo"}, 2).ToList())
                .RuleFor(r => r.MedicalConditions, f => f.PickRandom(new[] { "None", "Diabetes", "Hypertension", "Asthma" }, 1).ToList())
                .RuleFor(r => r.CreatedAt, f => f.Date.Past(1).ToUniversalTime())
                .RuleFor(r => r.UpdatedAt, f => f.Date.Recent(1).ToUniversalTime())
                .RuleFor(r => r.FamilyId, f => f.PickRandom(families.Select(f => f.Id)));

            var refugees = refugeeFaker.Generate(100);
            Refugees.AddRange(refugees);
        }
    }
}
