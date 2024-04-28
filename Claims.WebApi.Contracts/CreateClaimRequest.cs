using Claims.Domain.Contracts;
using Newtonsoft.Json;

namespace Claims.WebApi.Contracts
{
    public class CreateClaimRequest
    {
        [JsonProperty(PropertyName = "coverId")]
        public required string CoverId { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "name")]
        public required string Name { get; set; }

        [JsonProperty(PropertyName = "claimType")]
        public ClaimType Type { get; set; }

        [JsonProperty(PropertyName = "damageCost")]
        public decimal DamageCost { get; set; }

    }
}