using CareLink_Refugee.Persistence;
namespace CareLink_Refugee.Utils
{
    public static class DbInitializer
    {
        public static void Initialize(RefugeeDbContext context)
        {
            if (context.Database.EnsureCreated()){}

            if (context.Refugees.Any() || context.Shelters.Any())
                return;

            DataGenerator.GenerateMockData();

            context.Shelters.AddRange(DataGenerator.Shelters);
            context.SaveChangesAsync().Wait();
            context.Families.AddRange(DataGenerator.Families);
            context.SaveChangesAsync().Wait();

            foreach (var refugee in DataGenerator.Refugees)
            {
                var family = context.Families.FirstOrDefault(f => f.Id == refugee.FamilyId);
                if (family != null)
                {
                    refugee.FamilyId = family.Id;
                    refugee.AccomodationId = refugee.Accomodation.Id;
                }
            }
            context.Refugees.AddRange(DataGenerator.Refugees);
            context.SaveChangesAsync().Wait();
            
        }
    }
}
