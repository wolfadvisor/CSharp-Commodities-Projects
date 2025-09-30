using CalculadoraPreçoATermo.Services;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;

namespace PrecoTermoCalculator.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<decimal> ObterCDIAsync()
    {
        try
        {
            _logger.LogInformation("Obtendo taxa CDI via BrasilAPI");
            const string url = "https://brasilapi.com.br/api/cdi";
            var json = await _httpClient.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var valorStr = doc.RootElement.GetProperty("valor").GetString();
            var taxa = decimal.Parse(valorStr!, CultureInfo.InvariantCulture) / 100m;

            _logger.LogInformation("Taxa CDI obtida com sucesso: {Taxa}%", taxa * 100);
            return taxa;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter taxa CDI");
            throw new InvalidOperationException($"Erro ao obter CDI: {ex.Message}", ex);
        }
    }

    public async Task<decimal> ObterSelicAsync()
    {
        try
        {
            _logger.LogInformation("Tentando obter SELIC via API Banco Central");
            var taxa = await ObterSelicBancoCentralAsync();
            _logger.LogInformation("Taxa SELIC obtida com sucesso: {Taxa}%", taxa * 100);
            return taxa;
        }
        catch (Exception ex1)
        {
            _logger.LogWarning(ex1, "Falha na API do Banco Central, tentando BrasilAPI");
            try
            {
                var taxa = await ObterSelicBrasilApiAsync();
                _logger.LogInformation("Taxa SELIC obtida via BrasilAPI: {Taxa}%", taxa * 100);
                return taxa;
            }
            catch (Exception ex2)
            {
                _logger.LogError(ex2, "Erro ao obter SELIC em todas as APIs");
                throw new InvalidOperationException($"Erro ao obter SELIC: {ex2.Message}", ex2);
            }
        }
    }

    public async Task<(bool sucesso, decimal taxa, string fonte)> ObterTaxaAsync(TipoTaxa tipo)
    {
        try
        {
            return tipo switch
            {
                TipoTaxa.CDI => (true, await ObterCDIAsync(), "CDI (BrasilAPI)"),
                TipoTaxa.SELIC => (true, await ObterSelicAsync(), "SELIC (Banco Central/BrasilAPI)"),
                _ => (false, 0m, "Tipo não suportado")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter taxa {Tipo}", tipo);
            return (false, 0m, $"Erro: {ex.Message}");
        }
    }

    private async Task<decimal> ObterSelicBancoCentralAsync()
    {
        const string url = "https://api.bcb.gov.br/dados/serie/bcdata.sgs.11/dados/ultimos/1?formato=json";
        var json = await _httpClient.GetStringAsync(url);

        using var doc = JsonDocument.Parse(json);
        var valor = doc.RootElement[0].GetProperty("valor").GetString();
        return decimal.Parse(valor!, CultureInfo.InvariantCulture) / 100m;
    }

    private async Task<decimal> ObterSelicBrasilApiAsync()
    {
        const string url = "https://brasilapi.com.br/api/selic";
        var json = await _httpClient.GetStringAsync(url);

        using var doc = JsonDocument.Parse(json);
        var valorStr = doc.RootElement.GetProperty("valor").GetString();
        return decimal.Parse(valorStr!, CultureInfo.InvariantCulture) / 100m;
    }
}