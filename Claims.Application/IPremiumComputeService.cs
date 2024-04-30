using Claims.Domain.Contracts;

namespace Claims.Application
{
    public interface IPremiumComputeService
    {
        decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType);
    }
}