using System.Net.Http.Json;

namespace DPManagement.Infrastructure.Services;

public interface IViaCepService
{
    Task<ViaCepResponse?> GetEnderecoByCepAsync(string cep);
}

public class ViaCepService : IViaCepService
{
    private readonly HttpClient _httpClient;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ViaCepResponse?> GetEnderecoByCepAsync(string cep)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ViaCepResponse>();
                return (result == null || result.erro) ? null : result;
            }
        }
        catch
        {
            // Fallback: Silently return null so the user can edit manually
        }
        return null;
    }
}

public class ViaCepResponse
{
    public string cep { get; set; } = string.Empty;
    public string logradouro { get; set; } = string.Empty;
    public string complemento { get; set; } = string.Empty;
    public string bairro { get; set; } = string.Empty;
    public string localidade { get; set; } = string.Empty;
    public string uf { get; set; } = string.Empty;
    public bool erro { get; set; }
}
