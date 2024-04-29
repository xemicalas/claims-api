using Claims.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using Newtonsoft.Json;

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
        public async Task When_CreateGetAndRemoveCover_Expect_Success()
        {
            var (request, createCoverResponse) = await CreateCoverAsync(_client, null, null);
            createCoverResponse.EnsureSuccessStatusCode();

            var coverId = await createCoverResponse.Content.ReadAsStringAsync();

            var getCoverResponse = await _client.GetAsync($"/Covers/{coverId}");
            getCoverResponse.EnsureSuccessStatusCode();

            var getCoverResponseContent = await getCoverResponse.Content.ReadAsStringAsync();
            var cover = JsonConvert.DeserializeObject<GetCoverResponse>(getCoverResponseContent)!;

            Assert.Equal(coverId, cover.Id);
            Assert.Equal(request.StartDate.Date, cover.StartDate.Date);
            Assert.Equal(request.EndDate.Date, cover.EndDate.Date);
            //Assert.Equal(request.Type, cover.Type);
            Assert.True(cover.Premium > 0);

            var removeCoverResponse = await _client.DeleteAsync($"/Covers/{coverId}");
            removeCoverResponse.EnsureSuccessStatusCode();

            getCoverResponse = await _client.GetAsync($"/Covers/{coverId}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getCoverResponse.StatusCode);
        }

        public static async Task<(CreateCoverRequest, HttpResponseMessage)> CreateCoverAsync(HttpClient client, DateTime ?startDate, DateTime ?endDate)
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
