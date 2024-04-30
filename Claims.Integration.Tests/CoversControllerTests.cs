using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using Claims.Api.Contracts;
using Claims.Domain.Contracts;
using FluentAssertions;

namespace Claims.Integration.Tests
{
    public class CoversControllerTests
    {
        private readonly HttpClient _client;

        public CoversControllerTests()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_ => { });

            _client = application.CreateClient();
        }

        [Fact]
        public async Task When_ComputePremium_Expect_Success()
        {
            var request = new ComputePremiumRequest
            {
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(60)),
                CoverType = CoverType.ContainerShip
            };

            var computePremiumResponse = await _client.PostAsync("/ComputePremium", JsonContent.Create(request));
            computePremiumResponse.EnsureSuccessStatusCode();

            var premium = await computePremiumResponse.Content.ReadFromJsonAsync<ComputePremiumResponse>();

            premium!.Amount.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task When_GetCovers_Expect_Success()
        {
            var getCoversResponse = await _client.GetAsync("/Covers");
            getCoversResponse.EnsureSuccessStatusCode();

            var covers = await getCoversResponse.Content.ReadFromJsonAsync<List<GetCoverResponse>>();

            covers!.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task When_CreateCoverWithStartDateInThePast_Expect_BadRequest()
        {
            var (_, createCoverResponse) = await CreateCoverAsync(_client, DateTime.UtcNow.AddDays(-1), null);

            createCoverResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_CreateCoverWithExceedingPeriod_Expect_BadRequest()
        {
            var (_, createCoverResponse) = await CreateCoverAsync(_client, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddYears(2));

            createCoverResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_RemoveUnknownCover_Expect_NotFound()
        {
            var removeCoverResponse = await _client.DeleteAsync($"/Covers/{Guid.NewGuid()}");

            removeCoverResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_CreateGetAndRemoveCover_Expect_Success()
        {
            var (request, createCoverResponse) = await CreateCoverAsync(_client, null, null);
            createCoverResponse.EnsureSuccessStatusCode();

            var createdCoverResponse = await createCoverResponse.Content.ReadFromJsonAsync<CreatedCoverResponse>();

            var getCoverResponse = await _client.GetAsync($"/Covers/{createdCoverResponse!.Id}");
            getCoverResponse.EnsureSuccessStatusCode();

            var cover = await getCoverResponse.Content.ReadFromJsonAsync<GetCoverResponse>();

            cover!.Id.Should().Be(createdCoverResponse.Id);
            cover.StartDate.Date.Should().Be(request.StartDate.Date);
            cover.EndDate.Date.Should().Be(request.EndDate.Date);
            cover.Type.Should().Be(request.Type);
            cover.Premium.Should().BeGreaterThan(0);

            var removeCoverResponse = await _client.DeleteAsync($"/Covers/{createdCoverResponse.Id}");
            removeCoverResponse.EnsureSuccessStatusCode();

            getCoverResponse = await _client.GetAsync($"/Covers/{createdCoverResponse.Id}");

            getCoverResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        internal static async Task<(CreateCoverRequest, HttpResponseMessage)> CreateCoverAsync(HttpClient client, DateTime? startDate, DateTime? endDate)
        {
            CreateCoverRequest request = new()
            {
                StartDate = startDate ?? DateTime.UtcNow.AddHours(1),
                EndDate = endDate ?? DateTime.UtcNow.AddDays(6 * 30),
                Type = CoverType.PassengerShip,
            };

            return (request, await client.PostAsync("/Covers", JsonContent.Create(request)));
        }
    }
}
