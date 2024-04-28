using Claims.Auditing;
using Claims.Domain.Exceptions;
using Microsoft.Azure.Cosmos;

namespace Claims.Repositories.Repositories
{
    public class ClaimsRepository : IClaimsRepository
	{
        private readonly Container _container;

        public ClaimsRepository(Container container)
		{
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
		}

        public Task CreateClaimAsync(ClaimAudit claim)
        {
            return _container.CreateItemAsync(claim, new PartitionKey(claim.Id));
        }

        public Task DeleteClaimAsync(string id)
        {
            return _container.DeleteItemAsync<ClaimAudit>(id, new PartitionKey(id));
        }

        public async Task<ClaimAudit> GetClaimAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<ClaimAudit>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ClaimNotFoundException();
            }
        }

        public async Task<IEnumerable<ClaimAudit>> GetClaimsAsync()
        {
            var query = _container.GetItemQueryIterator<ClaimAudit>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<ClaimAudit>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results;
        }
    }
}

