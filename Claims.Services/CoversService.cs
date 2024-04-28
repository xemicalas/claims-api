using Claims.Domain;
using Claims.Repositories;

namespace Claims.Services
{
    public class CoverService : ICoversService
	{
        private readonly ICoversRepository _coversRepository;
        private readonly IPremiumComputeService _premiumComputeService;

        public CoverService(ICoversRepository coversRepository, IPremiumComputeService premiumComputeService)
		{
            _coversRepository = coversRepository;
            _premiumComputeService = premiumComputeService;
        }

        public Task CreateCoverAsync(Cover cover)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCoverAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Cover> GetCoverAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cover>> GetCoversAsync()
        {
            throw new NotImplementedException();
        }
    }
}

