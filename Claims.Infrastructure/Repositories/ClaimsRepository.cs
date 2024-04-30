using Claims.Domain.Exceptions;
using Claims.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Infrastructure.Repositories
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
            var claim = await Claims
                .Where(claim => claim.Id == id)
                .SingleOrDefaultAsync() ?? throw new ClaimNotFoundException();

            return claim;
        }

        public async Task<IEnumerable<ClaimEntity>> GetClaimsAsync()
        {
            return await Claims.ToListAsync();
        }
    }
}