using System.Net;
using System.Net.Http;
using System.Text;
using BirdMessage.Application.Dto;
using BirdMessage.Application.Externals;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BirdMessage.Tests.Services;

public class CepServiceTests
{
    [Fact]
    public async Task GetCepInfosAsync_WhenApiReturnsValidCep_ReturnsMappedDto()
    {
        var json = """
        {
          "cep": "01001-000",
          "logradouro": "Praça da Sé",
          "complemento": "lado ímpar",
          "unidade": "",
          "bairro": "Sé",
          "localidade": "São Paulo",
          "uf": "SP",
          "estado": "São Paulo",
          "regiao": "Sudeste",
          "ibge": "3550308",
          "gia": "1004",
          "ddd": "11",
          "siafi": "7107"
        }
        """;

        var handler = new StubHttpMessageHandler(json, HttpStatusCode.OK);
        var client = new HttpClient(handler);
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ExternalApis:CepApi"] = "https://viacep.com.br/ws/"
            })
            .Build();

        var service = new CepService(client, configuration);

        var result = await service.GetCepInfosAsync("01001000");

        Assert.Equal(new CepServiceDto(
            "01001-000",
            "Praça da Sé",
            "lado ímpar",
            "",
            "Sé",
            "São Paulo",
            "SP",
            "São Paulo",
            "Sudeste",
            "3550308",
            "1004",
            "11",
            "7107"), result);
    }

    private sealed class StubHttpMessageHandler(string responseContent, HttpStatusCode statusCode) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            });
        }
    }
}
