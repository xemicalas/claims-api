﻿using Claims.Domain.Exceptions;
using Claims.Repositories.Contracts;
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

        public Task CreateClaimAsync(ClaimEntity claim)
        {
            return _container.CreateItemAsync(claim, new PartitionKey(claim.Id));
        }

        public Task DeleteClaimAsync(string id)
        {
            return _container.DeleteItemAsync<ClaimEntity>(id, new PartitionKey(id));
        }

        public async Task<ClaimEntity> GetClaimAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<ClaimEntity>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ClaimNotFoundException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ClaimEntity>> GetClaimsAsync()
        {
            var query = _container.GetItemQueryIterator<ClaimEntity>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<ClaimEntity>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results;
        }
    }
}

