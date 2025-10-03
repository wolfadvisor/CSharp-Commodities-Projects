using CalculadoraPreçoATermo.Models;
using Microsoft.Extensions.Logging;

namespace PrecoTermoCalculator.Services
{
    public class CalculadoraService : ICalculadoraService
    {
        private readonly ILogger<CalculadoraService> _logger;
        private const int DiasUteisAno = 252;

        public CalculadoraService(ILogger<CalculadoraService> logger)
        {
            _logger = logger;
        }

        public ResultadoCalculo CalcularPrecoTermo(DadosCalculo dados)
        {
            _logger.LogInformation("Iniciando cálculo de preço a termo");

            var resultados = new ResultadoCalculo();

            resultados.ToneladasTotal = dados.QuantidadeSacas * dados.PesoSacaKg / 1000m;
            resultados.ValorSpotTotal = dados.PrecoSpot * dados.QuantidadeSacas;
            resultados.SetPrecoSpot(dados.PrecoSpot);

            // Correção do nome
            resultados.CustoArmazenagemTotal = CalcularCustoArmazenagem(dados);

            var tempoAnos = (decimal)dados.PrazoDias / DiasUteisAno;
            var custoArmazenagemRate = resultados.ValorSpotTotal > 0
                ? resultados.CustoArmazenagemTotal / resultados.ValorSpotTotal / tempoAnos
                : 0m;

            var taxaTotal = dados.TaxaJuros + custoArmazenagemRate;
            var fatorTermo = (decimal)Math.Pow((double)(1 + taxaTotal / DiasUteisAno), dados.PrazoDias);

            resultados.PrecoTermoUnitario = dados.PrecoSpot * fatorTermo;
            resultados.ValorTermoTotal = resultados.PrecoTermoUnitario * dados.QuantidadeSacas;

            if (resultados.ToneladasTotal > 0)
            {
                resultados.PrecoSpotPorTonelada = resultados.ValorSpotTotal / resultados.ToneladasTotal;
                resultados.PrecoTermoTonelada = resultados.ValorTermoTotal / resultados.ToneladasTotal;
            }

            resultados.CustoJurosTotal = resultados.ValorTermoTotal - resultados.ValorSpotTotal - resultados.CustoArmazenagemTotal;
            resultados.DiferencaTotal = resultados.ValorTermoTotal - resultados.ValorSpotTotal;

            _logger.LogInformation("Cálculo concluído - Preço termo: R$ {PrecoTermo}", resultados.PrecoTermoUnitario);

            return resultados;
        }

        public decimal CalcularCustoArmazenagem(DadosCalculo dados)
        {
            if (dados.CustoArmazenagem?.TemCustos != true)
                return 0m;

            decimal custoTotal = 0m;
            var meses = (decimal)dados.PrazoDias / 30m;
            var toneladas = dados.QuantidadeSacas * dados.PesoSacaKg / 1000m;
            var valorBase = dados.PrecoSpot * dados.QuantidadeSacas;

            custoTotal += dados.CustoArmazenagem.CustoFixoMensal * dados.QuantidadeSacas * meses;

            if (dados.CustoArmazenagem.CustoPercentualAnual > 0)
                custoTotal += valorBase * dados.CustoArmazenagem.CustoPercentualAnual * ((decimal)dados.PrazoDias / 365m);

            custoTotal += dados.CustoArmazenagem.CustoPorToneladaMes * toneladas * meses;

            if (dados.CustoArmazenagem.Seguro > 0)
                custoTotal += valorBase * dados.CustoArmazenagem.Seguro * ((decimal)dados.PrazoDias / 365m);

            custoTotal += dados.CustoArmazenagem.Manutencao * dados.QuantidadeSacas * meses;

            _logger.LogDebug("Custo armazenagem calculado: R$ {Custo}", custoTotal);

            return custoTotal;
        }
    }
}
