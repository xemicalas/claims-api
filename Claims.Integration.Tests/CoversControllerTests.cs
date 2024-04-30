using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using Newtonsoft.Json;
using Claims.Api.Contracts;
using Claims.Domain.Contracts;

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

            var responseContent = await computePremiumResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ComputePremiumResponse>(responseContent)!;

            Assert.True(response.Amount > 0);
        }

        [Fact]
        public async Task When_GetCovers_Expect_Success()
        {
            var getCoversResponse = await _client.GetAsync("/Covers");
            getCoversResponse.EnsureSuccessStatusCode();

            var responseContent = await getCoversResponse.Content.ReadAsStringAsync();
            var covers = JsonConvert.DeserializeObject<List<GetCoverResponse>>(responseContent)!;

            Assert.True(covers.Count() > 0);
        }

        [Fact]
        public async Task When_CreateCoverWithStartDateInThePast_Expect_BadRequest()
        {
            var (_, createCoverResponse) = await CreateCoverAsync(_client, DateTime.UtcNow.AddDays(-1), null);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createCoverResponse.StatusCode);
        }

        [Fact]
        public async Task When_CreateCoverWithExceedingPeriod_Expect_BadRequest()
        {
            var (_, createCoverResponse) = await CreateCoverAsync(_client, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddYears(2));
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createCoverResponse.StatusCode);
        }

        [Fact]
        public async Task When_RemoveUnknownCover_Expect_NotFound()
        {
            var removeCoverResponse = await _client.DeleteAsync($"/Covers/{Guid.NewGuid()}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, removeCoverResponse.StatusCode);
        }

        [Fact]
        public async Task When_CreateGetAndRemoveCover_Expect_Success()
        {
            var (request, createCoverResponse) = await CreateCoverAsync(_client, null, null);
            createCoverResponse.EnsureSuccessStatusCode();

            var createCoverResponseContent = await createCoverResponse.Content.ReadAsStringAsync();
            var createdCoverResponse = JsonConvert.DeserializeObject<CreatedCoverResponse>(createCoverResponseContent)!;

            var getCoverResponse = await _client.GetAsync($"/Covers/{createdCoverResponse.Id}");
            getCoverResponse.EnsureSuccessStatusCode();

            var getCoverResponseContent = await getCoverResponse.Content.ReadAsStringAsync();
            var cover = JsonConvert.DeserializeObject<GetCoverResponse>(getCoverResponseContent)!;

            Assert.Equal(createdCoverResponse.Id, cover.Id);
            Assert.Equal(request.StartDate.Date, cover.StartDate.Date);
            Assert.Equal(request.EndDate.Date, cover.EndDate.Date);
            //Assert.Equal(request.Type, cover.Type);
            Assert.True(cover.Premium > 0);

            var removeCoverResponse = await _client.DeleteAsync($"/Covers/{createdCoverResponse.Id}");
            removeCoverResponse.EnsureSuccessStatusCode();

            getCoverResponse = await _client.GetAsync($"/Covers/{createdCoverResponse.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getCoverResponse.StatusCode);
        }

        internal static async Task<(CreateCoverRequest, HttpResponseMessage)> CreateCoverAsync(HttpClient client, DateTime ?startDate, DateTime ?endDate)
        {
            CreateCoverRequest request = new()
            {
                StartDate = startDate.HasValue ? startDate.Value : DateTime.UtcNow.AddHours(1),
                EndDate = endDate.HasValue ? endDate.Value : DateTime.UtcNow.AddDays(6 * 30),
                Type = Domain.Contracts.CoverType.PassengerShip,
            };

            return (request, await client.PostAsync("/Covers", JsonContent.Create(request)));
        }
    }
}
