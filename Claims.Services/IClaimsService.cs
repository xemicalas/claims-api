using Claims.Domain;

namespace Claims.Services
{
	public interface IClaimsService
	{
        Task<IEnumerable<Claim>> GetClaimsAsync();
        Task<Claim> GetClaimAsync(string id);
        Task CreateClaimAsync(Claim item);
        Task DeleteClaimAsync(string id);
    }
}

