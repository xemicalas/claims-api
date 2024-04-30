using Claims.Infrastructure.Contracts;

namespace Claims.Infrastructure
{
    public interface IClaimsRepository
    {
        Task<IEnumerable<ClaimEntity>> GetClaimsAsync();
        Task<ClaimEntity> GetClaimAsync(string id);
        Task CreateClaimAsync(ClaimEntity item);
        Task DeleteClaimAsync(string id);
    }
}