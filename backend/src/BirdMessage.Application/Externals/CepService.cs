using System.Text.Json;
using BirdMessage.Application.Dto;
using BirdMessage.Application.Externals.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BirdMessage.Application.Externals
{
    public class CepService(HttpClient client, IConfiguration configuration) : ICepService
    {
        public async Task<CepServiceDto> GetCepInfosAsync(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
            {
                throw new ArgumentException("CEP is required.", nameof(cep));
            }

            var baseUrl = configuration["ExternalApis:CepApi"]
                ?? throw new InvalidOperationException("ExternalApis:CepApi configuration is missing.");

            var normalizedCep = cep.Trim();
            var url = $"{baseUrl.TrimEnd('/')}/{normalizedCep}/json/";

            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CepServiceDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            return result ?? throw new InvalidOperationException("Unable to deserialize CEP response.");
        }
    }
}