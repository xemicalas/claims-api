using Claims.Domain;

namespace Claims.Services
{
    public interface ICoversService
	{
        Task<IEnumerable<Cover>> GetCoversAsync();
        Task<Cover> GetCoverAsync(string id);
        Task CreateCoverAsync(Cover cover);
        Task DeleteCoverAsync(string id);
    }
}

