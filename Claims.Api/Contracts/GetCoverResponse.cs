using Claims.Domain.Contracts;

namespace Claims.Api.Contracts
{
    /// <summary>
    /// Get cover response
    /// </summary>
    public class GetCoverResponse
	{
        /// <summary>
        /// Cover identifier
        /// </summary>
        public required string Id { get; set; }

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
        public CoverType Type { get; set; }

        /// <summary>
        /// Cover premium amount
        /// </summary>
        public decimal Premium { get; set; }
    }
}

