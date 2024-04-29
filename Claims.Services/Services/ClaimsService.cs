using Claims.Domain.Contracts;
using Claims.Domain.Contracts.Exceptions;
using Claims.Repositories;
using Claims.Repositories.Contracts;
using Mapster;

namespace Claims.Services
{
    public class ClaimsService : IClaimsService
	{
        private readonly IClaimsRepository _claimsRepository;
        private readonly ICoversService _coversService;

        public ClaimsService(IClaimsRepository claimsRepository, ICoversService coversService)
		{
			_claimsRepository = claimsRepository;
            _coversService = coversService;
		}

        public async Task<string> CreateClaimAsync(Claim claim)
        {
            try
            {
                await _coversService.GetCoverAsync(claim.CoverId);
            }
            catch (CoverNotFoundException)
            {
                throw;
            }

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

