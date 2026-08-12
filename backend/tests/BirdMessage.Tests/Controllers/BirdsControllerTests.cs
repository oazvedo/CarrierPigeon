using System.Net;
using System.Net.Http.Json;
using BirdMessage.Application.Dto;
using BirdMessage.Domain.Common;
using Xunit;

namespace BirdMessage.Tests.Controllers;

public class BirdsControllerTests(BirdApiFactory factory) : IClassFixture<BirdApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private async Task<BirdResponseDto> CreateBirdAsync(string name = "Pombo")
    {
        var response = await _client.PostAsJsonAsync("/api/birds", new CreateBirdRequestDto(name, "rápido e confiável", 65));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<BirdResponseDto>())!;
    }

    [Fact]
    public async Task Create_ReturnsCreatedWithLocation()
    {
        var response = await _client.PostAsJsonAsync("/api/birds", new CreateBirdRequestDto("Coruja", "só entrega à noite", 45));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task GetById_ExistingBird_ReturnsOk()
    {
        var created = await CreateBirdAsync("Pavão");

        var response = await _client.GetAsync($"/api/birds/{created.Id}");
        var body = await response.Content.ReadFromJsonAsync<BirdResponseDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(created.Id, body!.Id);
    }

    [Fact]
    public async Task GetById_MissingBird_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/birds/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPaged_ReturnsOkWithCreatedBird()
    {
        var created = await CreateBirdAsync("Avestruz");

        var response = await _client.GetAsync("/api/birds?page=1&pageSize=50");
        var body = await response.Content.ReadFromJsonAsync<PaginatedResult<BirdResponseDto>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(body!.Items, b => b.Id == created.Id);
    }

    [Fact]
    public async Task Update_ExistingBird_ReturnsNoContent()
    {
        var created = await CreateBirdAsync("Galinha");

        var response = await _client.PutAsJsonAsync($"/api/birds/{created.Id}", new UpdateBirdRequestDto("Galinha Calma", "não entra mais em pânico", 20));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/birds/{created.Id}");
        var body = await getResponse.Content.ReadFromJsonAsync<BirdResponseDto>();
        Assert.Equal("Galinha Calma", body!.Name);
    }

    [Fact]
    public async Task Update_MissingBird_ReturnsNotFound()
    {
        var response = await _client.PutAsJsonAsync($"/api/birds/{Guid.NewGuid()}", new UpdateBirdRequestDto("X", "Y", 1));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingBird_ReturnsNoContentAndRemovesBird()
    {
        var created = await CreateBirdAsync("Urubu");

        var deleteResponse = await _client.DeleteAsync($"/api/birds/{created.Id}");
        var getResponse = await _client.GetAsync($"/api/birds/{created.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
