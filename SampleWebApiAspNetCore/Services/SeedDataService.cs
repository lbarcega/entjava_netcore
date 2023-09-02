using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(RockDbContext context)
        {
            context.RockItems.Add(new RockEntity() { Hardness = 10, Luster = "Adamantine", Color = "White", Name = "Diamond", Created = DateTime.Now });
            context.RockItems.Add(new RockEntity() { Hardness = 9, Luster = "Vitreous", Color = "Red", Name = "Ruby", Created = DateTime.Now });
            context.RockItems.Add(new RockEntity() { Hardness = 8, Luster = "Vitreous", Color = "Blue", Name = "Aquamarine", Created = DateTime.Now });
            context.RockItems.Add(new RockEntity() { Hardness = 2, Luster = "Dull", Color = "Red-orange", Name = "Cinnabar", Created = DateTime.Now });
            context.SaveChanges();
        }
    }
}
