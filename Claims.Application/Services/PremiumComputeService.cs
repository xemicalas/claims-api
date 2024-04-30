using Claims.Domain.Contracts;

namespace Claims.Application.Services
{
    public class PremiumComputeService : IPremiumComputeService
    {
        private const int BaseDayRate = 1250;
        private const int Month = 30;
        private const int HalfYear = 180;
        private const int Year = 365;

        private const int FirstPeriodDays = Month;
        private const int SecondPeriodDays = HalfYear - Month;
        private const int ThirdPeriodDays = Year - HalfYear;

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

            var premiumPerDay = BaseDayRate * multiplier;
            var daysLeftToCover = endDate.DayNumber - startDate.DayNumber;
            var totalPremium = 0m;

            var firtPeriodDays = Math.Min(FirstPeriodDays, daysLeftToCover);

            totalPremium += premiumPerDay * firtPeriodDays;
            daysLeftToCover -= FirstPeriodDays;

            if (daysLeftToCover > 0)
            {
                totalPremium += ComputePremiumForSecondPeriod(daysLeftToCover, premiumPerDay, coverType);
                daysLeftToCover -= SecondPeriodDays;
            }

            if (daysLeftToCover > 0)
            {
                totalPremium += ComputePremiumForThirdPeriod(daysLeftToCover, premiumPerDay, coverType);
            }

            return totalPremium;
        }

        private decimal ComputePremiumForSecondPeriod(int daysLeftToCover, decimal premiumPerDay, CoverType coverType)
        {
            var secondPeriodDays = Math.Min(SecondPeriodDays, daysLeftToCover);
            var discount = coverType == CoverType.Yacht ? 0.05m : 0.02m;

            var totalPremium = ComputePremiumWithDiscount(premiumPerDay, discount, secondPeriodDays);

            return totalPremium;
        }

        private decimal ComputePremiumForThirdPeriod(int daysLeftToCover, decimal premiumPerDay, CoverType coverType)
        {
            var thirdPeriodDays = Math.Min(ThirdPeriodDays, daysLeftToCover);
            var discount = coverType == CoverType.Yacht ? 0.08m : 0.03m;

            var totalPremium = ComputePremiumWithDiscount(premiumPerDay, discount, thirdPeriodDays);

            return totalPremium;
        }

        private decimal ComputePremiumWithDiscount(decimal premiumPerDay, decimal discount, int periodDays)
        {
            return (premiumPerDay - premiumPerDay * discount) * periodDays;
        }
    }
}