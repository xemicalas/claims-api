using Claims.Domain.Contracts;
using Claims.Infrastructure;
using Claims.Infrastructure.Contracts;
using Mapster;

namespace Claims.Application.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IClaimsRepository _claimsRepository;
        private readonly IAuditerService _auditerService;

        public ClaimsService(IClaimsRepository claimsRepository, IAuditerService auditerService)
        {
            _claimsRepository = claimsRepository;
            _auditerService = auditerService;
        }

        public async Task<string> CreateClaimAsync(Claim claim)
        {
            var claimId = Guid.NewGuid().ToString();
            claim.Id = claimId;

            await _claimsRepository.CreateClaimAsync(claim.Adapt<ClaimEntity>());

            await _auditerService.AuditClaimAsync(claimId, "POST");

            return claimId;
        }

        public async Task DeleteClaimAsync(string id)
        {
            await _claimsRepository.DeleteClaimAsync(id);
            await _auditerService.AuditClaimAsync(id, "DELETE");
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