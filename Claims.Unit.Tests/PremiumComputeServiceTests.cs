using System.Collections;
using Claims.Application.Services;
using Claims.Domain.Contracts;

namespace Claims.Unit.Tests
{
    public class PremiumComputeServiceTests
    {
        private readonly PremiumComputeService _service;

        public PremiumComputeServiceTests()
        {
            _service = new PremiumComputeService();
        }

        [Theory]
        [ClassData(typeof(PremiumComputeTestData))]
        public void TestComputePremium(string _, DateOnly startDate, DateOnly endDate, CoverType coverType, decimal expectedPremium)
        {
            var premium = _service.ComputePremium(startDate, endDate, coverType);
            Assert.Equal(expectedPremium, premium);
        }

        public class PremiumComputeTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Yacht
                yield return new object[]
                {
                "when period is less than or equal 30 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-05-31"),
                CoverType.Yacht,
                41250,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-06-01"),
                CoverType.Yacht,
                42556.25,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-28"),
                CoverType.Yacht,
                237187.5,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-29"),
                CoverType.Yacht,
                238452.5,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-01"),
                CoverType.Yacht,
                471212.5,
                };
                yield return new object[]
                {
                "when period is more than 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-02"),
                CoverType.Yacht,
                471212.5,
                };
                //PassengerShip
                yield return new object[]
                {
                "when period is less than or equal 30 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-05-31"),
                CoverType.PassengerShip,
                45000,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-06-01"),
                CoverType.PassengerShip,
                46470,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-28"),
                CoverType.PassengerShip,
                265500,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-29"),
                CoverType.PassengerShip,
                266955,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-01"),
                CoverType.PassengerShip,
                534675,
                };
                yield return new object[]
                {
                "when period is more than 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-02"),
                CoverType.PassengerShip,
                534675,
                };
                //ContainerShip
                yield return new object[]
                {
                "when period is less than or equal 30 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-05-31"),
                CoverType.ContainerShip,
                48750,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-06-01"),
                CoverType.ContainerShip,
                50342.5,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-28"),
                CoverType.ContainerShip,
                287625,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-29"),
                CoverType.ContainerShip,
                289201.25,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-01"),
                CoverType.ContainerShip,
                579231.25,
                };
                yield return new object[]
                {
                "when period is more than 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-02"),
                CoverType.ContainerShip,
                579231.25,
                };
                //BulkCarrier
                yield return new object[]
                {
                "when period is less than or equal 30 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-05-31"),
                CoverType.BulkCarrier,
                48750,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-06-01"),
                CoverType.BulkCarrier,
                50342.5,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-28"),
                CoverType.BulkCarrier,
                287625,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-29"),
                CoverType.BulkCarrier,
                289201.25,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-01"),
                CoverType.BulkCarrier,
                579231.25,
                };
                yield return new object[]
                {
                "when period is more than 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-02"),
                CoverType.BulkCarrier,
                579231.25,
                };
                //Tanker
                yield return new object[]
                {
                "when period is less than or equal 30 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-05-31"),
                CoverType.Tanker,
                56250.0,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-06-01"),
                CoverType.Tanker,
                58087.5,
                };
                yield return new object[]
                {
                "when period is more than 30 days, but less than or equal 180 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-28"),
                CoverType.Tanker,
                331875,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2024-10-29"),
                CoverType.Tanker,
                333693.75,
                };
                yield return new object[]
                {
                "when period is more than 180 days, but less than or equal 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-01"),
                CoverType.Tanker,
                668343.75,
                };
                yield return new object[]
                {
                "when period is more than 365 days",
                DateOnly.Parse("2024-05-01"),
                DateOnly.Parse("2025-05-02"),
                CoverType.Tanker,
                668343.75,
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}