using Claims.Domain.Contracts;
using Newtonsoft.Json;

namespace Claims.WebApi.Contracts
{
    /// <summary>
    /// Create cover request
    /// </summary>
    public class CreateCoverRequest
    {
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
    }
}