using Claims.Domain.Contracts;
using Newtonsoft.Json;

namespace Claims.WebApi.Contracts
{
    public class CreateCoverRequest
    {
        [JsonProperty(PropertyName = "startDate")]
        public DateOnly StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateOnly EndDate { get; set; }

        [JsonProperty(PropertyName = "claimType")]
        public CoverType Type { get; set; }

        [JsonProperty(PropertyName = "premium")]
        public decimal Premium { get; set; }
    }
}