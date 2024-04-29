using Claims.Domain.Contracts;
using Claims.Repositories;
using Claims.Repositories.Contracts;
using Mapster;

namespace Claims.Services
{
    public class ClaimsService : IClaimsService
	{
        private readonly IClaimsRepository _claimsRepository;

        public ClaimsService(IClaimsRepository claimsRepository)
		{
			_claimsRepository = claimsRepository;
		}

        public async Task<string> CreateClaimAsync(Claim claim)
        {
            var claimId = Guid.NewGuid().ToString();
            claim.Id = claimId;

            await _claimsRepository.CreateClaimAsync(claim.Adapt<ClaimEntity>());

            return claimId;
        }

        public Task DeleteClaimAsync(string id)
        {
            return _claimsRepository.DeleteClaimAsync(id);
        }

        public async Task<Claim> GetClaimAsync(string id)
        {
            var claimEntity = await _claimsRepository.GetClaimAsync(id);

            return claimEntity.Adapt<Claim>();
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            var claimEntities = await _claimsRepository.GetClaimsAsync();

            return claimEntities.Adapt<IEnumerable<Claim>>();
        }
    }
}

