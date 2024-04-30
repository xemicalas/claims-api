using Claims.Domain.Contracts;
using Claims.Infrastructure;
using Claims.Infrastructure.Contracts;
using Mapster;

namespace Claims.Application.Services
{
    public class CoversService : ICoversService
	{
        private readonly ICoversRepository _coversRepository;
        private readonly IPremiumComputeService _premiumComputeService;
        private readonly IAuditerService _auditerService;

        public CoversService(ICoversRepository coversRepository, IPremiumComputeService premiumComputeService, IAuditerService auditerService)
		{
            _coversRepository = coversRepository;
            _premiumComputeService = premiumComputeService;
            _auditerService = auditerService;
        }

        public async Task<string> CreateCoverAsync(Cover cover)
        {
            var coverId = Guid.NewGuid().ToString();
            var premium = _premiumComputeService.ComputePremium(DateOnly.FromDateTime(cover.StartDate), DateOnly.FromDateTime(cover.EndDate), cover.Type);
            cover.Id = coverId;
            cover.Premium = premium;

            await _coversRepository.CreateCoverAsync(cover.Adapt<CoverEntity>());
            await _auditerService.AuditCoverAsync(coverId, "POST");

            return coverId;
        }

        public async Task DeleteCoverAsync(string id)
        {
            await _coversRepository.DeleteCoverAsync(id);
            await _auditerService.AuditCoverAsync(id, "DELETE");
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