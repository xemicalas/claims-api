using Claims.Domain.Contracts;

namespace Claims.Services
{
    public class PremiumComputeService : IPremiumComputeService
    {
	    public PremiumComputeService()
	    {
	    }

        public decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            var multiplier = 1.3m;
            switch (coverType)
            {
                case CoverType.Yacht:
                    multiplier = 1.1m;
                    break;
                case CoverType.PassengerShip:
                    multiplier = 1.2m;
                    break;
                case CoverType.Tanker:
                    multiplier = 1.5m;
                    break;
            }

            var premiumPerDay = 1250 * multiplier;
            var daysLeftToCover = endDate.DayNumber - startDate.DayNumber;
            var totalPremium = 0m;

            var firtPeriodDays = Math.Min(30, daysLeftToCover);

            totalPremium += premiumPerDay * firtPeriodDays;
            daysLeftToCover -= 30;

            if (daysLeftToCover > 0)
            {
                var secondPeriodDays = Math.Min(180 - 30, daysLeftToCover);
                var discount = coverType == CoverType.Yacht ? 0.05m : 0.02m;

                totalPremium += (premiumPerDay - premiumPerDay * discount) * secondPeriodDays;
                daysLeftToCover -= 180 - 30;
            }

            if (daysLeftToCover > 0)
            {
                var thirdPeriodDays = Math.Min(365 - 180, daysLeftToCover);
                var discount = coverType == CoverType.Yacht ? 0.08m : 0.03m;

                totalPremium += (premiumPerDay - premiumPerDay * discount) * thirdPeriodDays;
            }

            return totalPremium;
        }
    }
}

