using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using src.Infra;
using src.Model;
using Xunit;

namespace test;

public class MainControllerTest
{
    private IConfiguration _configuration;
    private readonly Faker _faker;
    private readonly RedisDb _db;

    public MainControllerTest()
    {
        SetConfig();

        _faker = new();

        _db = new(_configuration);
    }
    
    [Fact]
    public async Task ShouldSaveResult()
    {
        var key = _faker.Random.Number(100000, 999999).ToString();

        var cacheKey = _db.GetCacheKey(key);

        var firstOutput = _db.GetStringAsync<Result>(cacheKey);

        firstOutput.Result.Should().BeNull();

        await GetHttpResponseAsync(key);

        var secondOutput = await _db.GetStringAsync<Result>(cacheKey);

        secondOutput.Should().NotBeNull();
        secondOutput.Message.Should().Be("Data saved");
    }

    [Fact]
    public async Task ShouldFindResult()
    {
        var key = _faker.Random.Number(100000, 999999).ToString();

        var cacheKey = _db.GetCacheKey(key);

        await _db.SetStringAsync(cacheKey, new Result("Data found"));

        var response = await GetHttpResponseAsync(key);

        var output = await response.Content.ReadFromJsonAsync<Result>();

        output.Should().NotBeNull();
        output.Message.Should().Be("Data found");
    }

    private void SetConfig()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    }

    private static async Task<HttpResponseMessage> GetHttpResponseAsync(string cacheKey)
    {
        var endpoint = $"http://localhost:5054/generate-result/{cacheKey}";

        using var factory = new WebApplicationFactory<Program>();

        using var httpClient = ConfigureClient(factory);

        return await httpClient.GetAsync(endpoint);
    }

    private static HttpClient ConfigureClient(WebApplicationFactory<Program> factory)
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services => 
            {
                services.AddHttpsRedirection(options => options.HttpsPort = 5016);
            });
        }).CreateClient();

        httpClient.DefaultRequestHeaders.Add("User-Agent", "IntegrationTest");

        return httpClient;
    }
}
