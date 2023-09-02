using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class RockDbContext : DbContext
    {
        public RockDbContext(DbContextOptions<RockDbContext> options)
            : base(options)
        {
        }

        public DbSet<RockEntity> RockItems { get; set; } = null!;
    }
}
