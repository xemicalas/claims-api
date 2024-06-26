﻿using System.Net.Http.Json;
using Claims.Api.Contracts;
using Claims.Domain.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;

namespace Claims.Integration.Tests
{
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

            var claims = await getClaimsResponse.Content.ReadFromJsonAsync<List<GetClaimResponse>>();

            claims!.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task When_CreateWithDamageExceedingLimit_Expect_BadRequest()
        {
            var (_, createClaimResponse) = await CreateClaimAsync(_client, Guid.NewGuid().ToString(), null, 100001);

            createClaimResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_CreateClaimWithNotExistingCoverId_Expect_BadRequest()
        {
            var (_, createClaimResponse) = await CreateClaimAsync(_client, Guid.NewGuid().ToString(), null);

            createClaimResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_CreateClaimWithNotInCoverPeriod_Expect_BadRequest()
        {
            var (_, createCoverResponse) = await CoversControllerTests.CreateCoverAsync(_client, null, null);
            createCoverResponse.EnsureSuccessStatusCode();
            var createdCoverResponse = await createCoverResponse.Content.ReadFromJsonAsync<CreatedCoverResponse>();

            var (_, createClaimResponse) = await CreateClaimAsync(_client, createdCoverResponse!.Id, DateTime.UtcNow.AddYears(-1));

            createClaimResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_RemoveUnknownClaim_Expect_NotFound()
        {
            var removeClaimResponse = await _client.DeleteAsync($"/Claims/{Guid.NewGuid()}");

            removeClaimResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_CreateGetAndRemoveClaim_Expect_Success()
        {
            var (_, createCoverResponse) = await CoversControllerTests.CreateCoverAsync(_client, null, null);
            createCoverResponse.EnsureSuccessStatusCode();
            var createdCoverResponse = await createCoverResponse.Content.ReadFromJsonAsync<CreatedCoverResponse>();

            var (request, createClaimResponse) = await CreateClaimAsync(_client, createdCoverResponse!.Id, null);
            createClaimResponse.EnsureSuccessStatusCode();
            var createdClaimResponse = await createClaimResponse.Content.ReadFromJsonAsync<CreatedClaimResponse>();

            var getClaimResponse = await _client.GetAsync($"/Claims/{createdClaimResponse!.Id}");
            getClaimResponse.EnsureSuccessStatusCode();
            var claim = await getClaimResponse.Content.ReadFromJsonAsync<GetClaimResponse>();

            claim!.Id.Should().Be(createdClaimResponse.Id);
            claim.CoverId.Should().Be(request.CoverId);
            claim.Created.Date.Should().Be(request.Created.Date);
            claim.Name.Should().Be(request.Name);
            claim.Type.Should().Be(request.Type);
            claim.DamageCost.Should().Be(request.DamageCost);

            var removeClaimResponse = await _client.DeleteAsync($"/Claims/{createdClaimResponse.Id}");
            removeClaimResponse.EnsureSuccessStatusCode();

            getClaimResponse = await _client.GetAsync($"/Claims/{createdClaimResponse.Id}");

            getClaimResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        internal static async Task<(CreateClaimRequest, HttpResponseMessage)> CreateClaimAsync(HttpClient client, string coverId, DateTime? created, decimal damageCost = 50000)
        {
            CreateClaimRequest request = new()
            {
                CoverId = coverId,
                Created = created ?? DateTime.UtcNow.AddHours(1),
                Name = Guid.NewGuid().ToString(),
                Type = ClaimType.BadWeather,
                DamageCost = damageCost
            };

            return (request, await client.PostAsync("/Claims", JsonContent.Create(request)));
        }
    }
}