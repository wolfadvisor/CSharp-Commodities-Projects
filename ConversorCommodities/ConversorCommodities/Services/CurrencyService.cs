using System.Net.Http.Json;
using System.Text.Json;
using ConversorCommodities.Models;

namespace ConversorCommodities.Services
{
    /// <summary>
    /// Serviço responsável por buscar cotações de moeda do Banco Central
    /// </summary>
    public class CurrencyService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed = false;

        public CurrencyService()
        {
            _httpClient = new HttpClient();
            // Configurar timeout para evitar travamentos
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Obtém a cotação do dólar do dia atual via API do Banco Central
        /// </summary>
        /// <returns>Valor da cotação de compra do dólar</returns>
        /// <exception cref="HttpRequestException">Erro na requisição HTTP</exception>
        /// <exception cref="InvalidOperationException">Dados não encontrados</exception>
        public async Task<decimal> ObterCotacaoDolarAsync()
        {
            try
            {
                string hoje = DateTime.Now.ToString("MM-dd-yyyy");
                string url = $"https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/CotacaoDolarDia(dataCotacao=@data)?@data='{hoje}'&$top=1&$format=json";

                Console.WriteLine("Buscando cotação do dólar...");

                var response = await _httpClient.GetFromJsonAsync<CotacaoResponse>(url);

                if (response?.Value == null || response.Value.Length == 0)
                {
                    throw new InvalidOperationException("Cotação não encontrada para hoje. Pode ser fim de semana ou feriado.");
                }

                var cotacaoItem = response.Value[0];
                if (cotacaoItem == null)
                {
                    throw new InvalidOperationException("Dados de cotação inválidos.");
                }

                return cotacaoItem.cotacaoCompra; // Corrigido para usar a propriedade correta
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Erro ao conectar com a API do Banco Central: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erro ao processar dados da cotação: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Converte valor de Real para Dólar
        /// </summary>
        public async Task<(decimal valorConvertido, decimal cotacaoUsada)> ConverterRealParaDolarAsync(decimal valorReais)
        {
            var cotacao = await ObterCotacaoDolarAsync();
            return (valorReais / cotacao, cotacao);
        }

        /// <summary>
        /// Converte valor de Dólar para Real
        /// </summary>
        public async Task<(decimal valorConvertido, decimal cotacaoUsada)> ConverterDolarParaRealAsync(decimal valorDolar)
        {
            var cotacao = await ObterCotacaoDolarAsync();
            return (valorDolar * cotacao, cotacao);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}