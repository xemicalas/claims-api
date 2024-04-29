using System.Net.Http.Json;
using Claims.Domain.Contracts;
using Claims.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Claims.Integration.Tests;

public class ClaimsControllerTests
{
    private readonly HttpClient _client;

    public ClaimsControllerTests()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(_ => { });

        _client = application.CreateClient();
    }

    [Fact]
    public async Task When_GetClaims_Expect_Success()
    {
        var getClaimsResponse = await _client.GetAsync("/Claims");
        getClaimsResponse.EnsureSuccessStatusCode();

        var responseContent = await getClaimsResponse.Content.ReadAsStringAsync();
        var claims = JsonConvert.DeserializeObject<List<GetClaimResponse>>(responseContent)!;

        Assert.True(claims.Count() > 0);
    }

    [Fact]
    public async Task When_CreateClaimWithNotExistingCoverId_Expect_NotFound()
    {
        CreateClaimRequest request = new()
        {
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow,
            Name = Guid.NewGuid().ToString(),
            Type = ClaimType.BadWeather,
            DamageCost = 50000
        };

        var createClaimResponse = await CreateClaimAsync(_client, request);

        Assert.Equal(System.Net.HttpStatusCode.NotFound, createClaimResponse.StatusCode);
    }

    [Fact]
    public async Task When_CreateGetAndRemoveClaim_Expect_Success()
    {
        var createCoverResponse = await CoversControllerTests.CreateCoverAsync(_client);
        createCoverResponse.EnsureSuccessStatusCode();
        var coverId = await createCoverResponse.Content.ReadAsStringAsync();

        CreateClaimRequest request = new()
        {
            CoverId = coverId,
            Created = DateTime.UtcNow,
            Name = Guid.NewGuid().ToString(),
            Type = ClaimType.BadWeather,
            DamageCost = 50000
        };

        var createClaimResponse = await CreateClaimAsync(_client, request);
        createClaimResponse.EnsureSuccessStatusCode();
        var claimId = await createClaimResponse.Content.ReadAsStringAsync();

        var getClaimResponse = await _client.GetAsync($"/Claims/{claimId}");
        getClaimResponse.EnsureSuccessStatusCode();
        var getClaimResponseContent = await getClaimResponse.Content.ReadAsStringAsync();
        var claim = JsonConvert.DeserializeObject<GetClaimResponse>(getClaimResponseContent)!;

        Assert.Equal(claimId, claim.Id);
        Assert.Equal(request.CoverId, claim.CoverId);
        Assert.Equal(request.Created.Date, claim.Created.Date);
        Assert.Equal(request.Name, claim.Name);
        //Assert.Equal(request.Type, claim.Type);
        Assert.Equal(request.DamageCost, claim.DamageCost);

        var removeClaimResponse = await _client.DeleteAsync($"/Claims/{claimId}");
        removeClaimResponse.EnsureSuccessStatusCode();

        getClaimResponse = await _client.GetAsync($"/Claims/{claimId}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getClaimResponse.StatusCode);
    }

    private static async Task<HttpResponseMessage> CreateClaimAsync(HttpClient client, CreateClaimRequest request)
    {
        return await client.PostAsync("/Claims", JsonContent.Create(request));
    }
}
