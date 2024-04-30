using Claims.Domain.Contracts;
namespace Claims.Api.Contracts
{
    /// <summary>
    /// Get claim response
    /// </summary>
    public class GetClaimResponse
    {
        /// <summary>
        /// Claim identifier
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Cover identifier, used to match claim with the cover
        /// </summary>
        public required string CoverId { get; set; }

        /// <summary>
        /// The date when claim was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Claim name or description
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Claim type
        /// </summary>
        public ClaimType Type { get; set; }

        /// <summary>
        /// Damage cost of a claim
        /// </summary>
        public decimal DamageCost { get; set; }
    }
}