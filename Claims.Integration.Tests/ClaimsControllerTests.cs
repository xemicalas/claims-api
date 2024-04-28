using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Claims.Integration.Tests;

public class ClaimsControllerTests
{
    [Fact]
    public async Task Get_Claims()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(_ =>
            { });

        var client = application.CreateClient();

        var response = await client.GetAsync("/Claims");

        response.EnsureSuccessStatusCode();

        //TODO: Apart from ensuring 200 OK being returned, what else can be asserted?
    }
}
