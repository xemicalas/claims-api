using Claims.Domain.Exceptions;
using Claims.Repositories.Contracts;
using Microsoft.Azure.Cosmos;

namespace Claims.Repositories.Repositories
{
    public class CoversRepository : ICoversRepository
	{
        private readonly Container _container;

        public CoversRepository(Container container)
		{
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public Task CreateCoverAsync(CoverEntity cover)
        {
            return _container.CreateItemAsync(cover, new PartitionKey(cover.Id));
        }

        public Task DeleteCoverAsync(string id)
        {
            return _container.DeleteItemAsync<CoverEntity>(id, new(id));
        }

        public async Task<CoverEntity> GetCoverAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<CoverEntity>(id, new(id));
                return response.Resource;

            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new CoverNotFoundException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CoverEntity>> GetCoversAsync()
        {
            var query = _container.GetItemQueryIterator<CoverEntity>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<CoverEntity>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results;
        }
    }
}

