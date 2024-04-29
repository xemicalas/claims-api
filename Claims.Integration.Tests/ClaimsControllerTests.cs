using System.Net.Http.Json;
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
    public async Task Get_Claims()
    {
        var getClaimsResponse = await _client.GetAsync("/Claims");
        getClaimsResponse.EnsureSuccessStatusCode();

        var responseContent = await getClaimsResponse.Content.ReadAsStringAsync();
        var claims = JsonConvert.DeserializeObject<List<GetClaimResponse>>(responseContent)!;

        Assert.True(claims.Count() > 0);
    }

    [Fact]
    public async Task Create_Get_And_Remove_Claim()
    {
        CreateClaimRequest request = new() {
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow,
            Name = Guid.NewGuid().ToString(),
            Type = Domain.Contracts.ClaimType.BadWeather,
            DamageCost = 50000
        };

        var createClaimResponse = await _client.PostAsync("/Claims", JsonContent.Create(request));
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
}
