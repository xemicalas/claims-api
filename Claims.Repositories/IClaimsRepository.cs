using Claims.Auditing;

namespace Claims.Repositories
{
    public interface IClaimsRepository
	{
        Task<IEnumerable<ClaimAudit>> GetClaimsAsync();
        Task<ClaimAudit> GetClaimAsync(string id);
        Task CreateClaimAsync(ClaimAudit item);
        Task DeleteClaimAsync(string id);
    }
}

