using Claims.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Repositories.Repositories
{
    public class ClaimsRepository : DbContext, IClaimsRepository
	{
        private DbSet<ClaimEntity> Claims { get; init; }

        public ClaimsRepository(DbContextOptions<ClaimsRepository> options)
            : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClaimEntity>().ToCollection("claims");
        }

        public async Task CreateClaimAsync(ClaimEntity claim)
        {
            Claims.Add(claim);
            await SaveChangesAsync();
        }

        public async Task DeleteClaimAsync(string id)
        {
            var claim = await GetClaimAsync(id);
            if (claim is not null)
            {
                Claims.Remove(claim);
                await SaveChangesAsync();
            }
        }

        public async Task<ClaimEntity> GetClaimAsync(string id)
        {
            return await Claims
                .Where(claim => claim.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ClaimEntity>> GetClaimsAsync()
        {
            return await Claims.ToListAsync();
        }
    }
}

