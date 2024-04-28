using Claims.Domain.Contracts;
using Claims.Repositories;
using Claims.Repositories.Contracts;
using Mapster;

namespace Claims.Services
{
    public class CoversService : ICoversService
	{
        private readonly ICoversRepository _coversRepository;
        private readonly IPremiumComputeService _premiumComputeService;

        public CoversService(ICoversRepository coversRepository, IPremiumComputeService premiumComputeService)
		{
            _coversRepository = coversRepository;
            _premiumComputeService = premiumComputeService;
        }

        public async Task<string> CreateCoverAsync(Cover cover)
        {
            var coverId = Guid.NewGuid().ToString();
            var premium = _premiumComputeService.ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
            cover.Id = coverId;
            cover.Premium = premium;

            await _coversRepository.CreateCoverAsync(cover.Adapt<CoverEntity>());

            return coverId;
        }

        public Task DeleteCoverAsync(string id)
        {
            return _coversRepository.DeleteCoverAsync(id);
        }

        public async Task<Cover> GetCoverAsync(string id)
        {
            var coverEntity = await _coversRepository.GetCoverAsync(id);

            return coverEntity.Adapt<Cover>();
        }

        public async Task<IEnumerable<Cover>> GetCoversAsync()
        {
            var coverEntities = await _coversRepository.GetCoversAsync();

            return coverEntities.Adapt<IEnumerable<Cover>>();
        }
    }
}

