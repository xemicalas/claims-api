using Claims.Domain.Contracts;

namespace Claims.Application
{
    public interface IClaimsService
	{
        Task<IEnumerable<Claim>> GetClaimsAsync();
        Task<Claim> GetClaimAsync(string id);
        Task<string> CreateClaimAsync(Claim claim);
        Task DeleteClaimAsync(string id);
    }
}