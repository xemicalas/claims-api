using Newtonsoft.Json;

namespace Claims
{
    public class Claim
    {
        [JsonProperty(PropertyName = "id")]
        public string ?Id { get; set; }

        [JsonProperty(PropertyName = "coverId")]
        public string CoverId { get; set; } = null!;

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = null!;

        [JsonProperty(PropertyName = "claimType")]
        public ClaimType Type { get; set; }

        [JsonProperty(PropertyName = "damageCost")]
        public decimal DamageCost { get; set; }

    }
}