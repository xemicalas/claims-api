using System.Text.Json.Serialization;
using Claims.Domain.Contracts;

namespace Claims.Api.Contracts
{
    /// <summary>
    /// Create cover request
    /// </summary>
    public class CreateCoverRequest
    {
        /// <summary>
        /// Start date of the cover
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the cover
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Cover type
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CoverType Type { get; set; }
    }
}