using Claims.Domain.Contracts;

namespace Claims.Api.Contracts
{
    /// <summary>
    /// Compute premium request
    /// </summary>
    public class ComputePremiumRequest
    {
        /// <summary>
        /// Cover's start date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Cover's end date
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Cover type
        /// </summary>
        public CoverType CoverType { get; set; }
    }
}

