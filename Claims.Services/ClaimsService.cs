using Claims.Domain;
using Claims.Repositories;

namespace Claims.Services
{
    public class ClaimsService : IClaimsService
	{
        private readonly IClaimsRepository _claimsRepository;

        public ClaimsService(IClaimsRepository claimsRepository)
		{
			_claimsRepository = claimsRepository;
		}

        public Task CreateClaimAsync(Claim item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClaimAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Claim> GetClaimAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            throw new NotImplementedException();
        }
    }
}

