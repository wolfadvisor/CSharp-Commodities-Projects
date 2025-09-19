using System.Text.Json.Serialization;

namespace ConversorCommodities.Models
{
    /// <summary>
    /// Modelo para deserialização da resposta da API de cotações de cambio.
    /// </summary>

    public class CotacaoResponse
    {
        [JsonPropertyName("value")]
        public CotacaoItem[] Value { get; set; } = Array.Empty<CotacaoItem>();
    
    }

    public class CotacaoItem
    {
        [JsonPropertyName("cotacaoCompra")]
        public decimal cotacaoCompra { get; set; }

        [JsonPropertyName("cotacaoVenda")]
        public decimal cotacaoVenda { get; set; }

        [JsonPropertyName("dataHoraCotacao")]
        public string dataHoraCotacao { get; set; }
    }

}
