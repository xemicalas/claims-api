using Claims.Infrastructure.Contracts;

namespace Claims.Infrastructure
{
    public interface ICoversRepository
    {
        Task<IEnumerable<CoverEntity>> GetCoversAsync();
        Task<CoverEntity> GetCoverAsync(string id);
        Task CreateCoverAsync(CoverEntity cover);
        Task DeleteCoverAsync(string id);
    }
}