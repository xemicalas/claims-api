using Claims.Domain.Contracts;
using Newtonsoft.Json;

namespace Claims.WebApi.Contracts
{
    /// <summary>
    /// Get cover response
    /// </summary>
    public class GetCoverResponse
	{
        /// <summary>
        /// Cover identifier
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public required string Id { get; set; }

        /// <summary>
        /// Start date of the cover
        /// </summary>
        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the cover
        /// </summary>
        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate { get; set; }


        /// <summary>
        /// Cover type
        /// </summary>
        [JsonProperty(PropertyName = "coverType")]
        public CoverType Type { get; set; }

        /// <summary>
        /// Cover premium amount
        /// </summary>
        [JsonProperty(PropertyName = "premium")]
        public decimal Premium { get; set; }
    }
}

