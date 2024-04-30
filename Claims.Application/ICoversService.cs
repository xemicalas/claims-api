using Claims.Domain.Contracts;

namespace Claims.Application
{
    public interface ICoversService
	{
        Task<IEnumerable<Cover>> GetCoversAsync();
        Task<Cover> GetCoverAsync(string id);
        Task<string> CreateCoverAsync(Cover cover);
        Task DeleteCoverAsync(string id);
    }
}