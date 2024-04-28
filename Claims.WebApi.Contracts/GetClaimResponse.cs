using Claims.Domain.Contracts;
using Newtonsoft.Json;

namespace Claims.WebApi.Contracts
{
    /// <summary>
    /// Get claim response
    /// </summary>
    public class GetClaimResponse
	{
        /// <summary>
        /// Claim identifier
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public required string Id { get; set; }

        /// <summary>
        /// Cover identifier, used to match claim with the cover
        /// </summary>
        [JsonProperty(PropertyName = "coverId")]
        public required string CoverId { get; set; }

        /// <summary>
        /// The date when claim was created
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Claim name or description
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public required string Name { get; set; }

        /// <summary>
        /// Claim type
        /// </summary>
        [JsonProperty(PropertyName = "claimType")]
        public ClaimType Type { get; set; }

        /// <summary>
        /// Damage cost of a claim
        /// </summary>
        [JsonProperty(PropertyName = "damageCost")]
        public decimal DamageCost { get; set; }
    }
}

