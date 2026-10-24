using System.Net;
using System.Net.Http.Json;
using Jacob_C.IntegrationTests.Infrastructure;
using Jacob_C.Shared.Features.Examples;

namespace Jacob_C.IntegrationTests.Features.Examples.Create;

public sealed class CreateExampleTests : IClassFixture<ExampleApiFixture>
{
    private readonly ExampleApiFixture _fixture;

    public CreateExampleTests(ExampleApiFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Post_CreatesExample_AndCanReadItBack()
    {
        await _fixture.ResetDatabaseAsync();
        var request = new ExampleRequest("Integration example", "integration@example.com");

        using var response = await _fixture.Client.PostAsJsonAsync("/api/examples", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<ExampleResponse>();
        Assert.NotNull(created);
        Assert.Equal(request.Name, created.Name);
        Assert.Equal(request.Email, created.Email);

        var saved = await _fixture.Client.GetFromJsonAsync<ExampleResponse>($"/api/examples/{created.Id}");
        Assert.Equal(created, saved);
    }
}
