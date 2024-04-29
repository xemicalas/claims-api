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
    public async Task When_CreateWithDamageExceedingLimit_Expect_BadRequest()
    {
        var (_, createClaimResponse) = await CreateClaimAsync(_client, Guid.NewGuid().ToString(), null, 100001);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, createClaimResponse.StatusCode);
    }

    [Fact]
    public async Task When_CreateClaimWithNotExistingCoverId_Expect_BadRequest()
    {
        var (_, createClaimResponse) = await CreateClaimAsync(_client, Guid.NewGuid().ToString(), null);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, createClaimResponse.StatusCode);
    }

    [Fact]
    public async Task When_CreateClaimWithNotInCoverPeriod_Expect_BadRequest()
    {
        var (_, createCoverResponse) = await CoversControllerTests.CreateCoverAsync(_client, null, null);
        createCoverResponse.EnsureSuccessStatusCode();
        var coverId = await createCoverResponse.Content.ReadAsStringAsync();

        var (_, createClaimResponse) = await CreateClaimAsync(_client, coverId, DateTime.UtcNow.AddYears(-1));
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, createClaimResponse.StatusCode);
    }

    [Fact]
    public async Task When_CreateGetAndRemoveClaim_Expect_Success()
    {
        var (_, createCoverResponse) = await CoversControllerTests.CreateCoverAsync(_client, null, null);
        createCoverResponse.EnsureSuccessStatusCode();
        var coverId = await createCoverResponse.Content.ReadAsStringAsync();

        var (request, createClaimResponse) = await CreateClaimAsync(_client, coverId, null);
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

    private static async Task<(CreateClaimRequest, HttpResponseMessage)> CreateClaimAsync(HttpClient client, string coverId, DateTime? created, decimal damageCost = 50000)
    {
        CreateClaimRequest request = new()
        {
            CoverId = coverId,
            Created = created.HasValue ? created.Value : DateTime.UtcNow.AddHours(1),
            Name = Guid.NewGuid().ToString(),
            Type = ClaimType.BadWeather,
            DamageCost = damageCost
        };

        return (request, await client.PostAsync("/Claims", JsonContent.Create(request)));
    }
}
