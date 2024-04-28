using Claims.Domain.Contracts;

namespace Claims.Services
{
    public interface IPremiumComputeService
    {
        decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType);
    }
}