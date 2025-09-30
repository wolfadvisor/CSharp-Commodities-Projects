using CalculadoraPreçoATermo.Models;
using Microsoft.Extensions.Logging;
using PrecoTermoCalculator;

namespace PrecoTermoCalculator.Services;

public class CalculadoraService : ICalculadoraService
{
    private readonly ILogger<CalculadoraService> _logger;
    private const int DiasUteisAno = 252;

    public CalculadoraService(ILogger<CalculadoraService> logger)
    {
        _logger = logger;
    }

    public ResultadoCalculo CalcularPrecotermo(DadosCalculo dados)
    {
        _logger.LogInformation("Iniciando o calculo do preço a termo:");

        var resultados = new ResultadoCalculos();

        //Conversões basicas
        resultados.ToneladasTotal = dados.QuantidadeSacas * dados.PesoSacakg / 1000m;
        resultados.ValorSpotTotal = dados.PrecoSpot * dados.QuantidadeSacas;
        resultados.SetPrecoSpot(dados.PrecoSpot);

        //Calculo do custo da armazenagem

        resultados.CustoArmagenazemTotal = CalcularCustoArmazenagem(dados);


        //calculo do preço a termo
        var tempoAnos = (decimal)dados.PrazoDias / DiasUteisAno;
        var custoArmazenagemRate = resultados.ValorSpotTotal > 0 ? resultados.CustoArmagenazemTotal / resultados.ValorSpotTotal / tempoAnos : 0m;

        var taxaTotal = dados.TaxaJuros + CustoArmagenazemRate;

        //Usando o compounding diario para maior precisão

        var fatorTermo = (decimal)Math.Pow((double)(1 + taxaTotal / DiasUteisAno), dados.PrazoDias);

        resultados.PrecoTermoUnitario = dados.PrecoSpot * fatorTermo;
        resultados.ValorTermoTotal = resultados.PrecoTermoUnitario * dados.QuantidadeSacas;


        //Calculos por Tonelada
        if (resultados.ToneladasTotal > 0)
        {
            resultados.PrecoSpotPorTonelada = resultados.ValorSpotTotal / resultados.ToneladasTotal;
            resultados.PrecoTermoTonelada = resultados.ValorTermoTotal / resultados.ToneladasTotal;
        }

        //Custos detalhados 
        resultados.CustoJurosTotal = resultados.ValorTermoTotal - resultados.ValorSpotTotal - resultados.CustoArmagenazemTotal;
        resultados.DiferencaTotal = resultados.ValorTermoTotal - resultados.ValorSpotTotal;

        _logger.LogInformation($"Calculo concluído - Preço termo:R$ {PrecoTermo}", resultados.PrecoTermoUnitario);
        return resultados;
    }
    public decimal CalcularCustoArmazenagem(DadosCalculo dados)
    {
        if (dados.custoArmazenagem?.TemCustos != true)
        { return 0m;
        }

        decimal custoTotal = 0m;
        var meses = (decimal)dados.PrazoDias / 30m;
        var toneladas = dados.QuantidadeSacas * dados.PesoSacakg / 1000m;
        var valorBase = dados.PrecoSpot * dados.QuantidadeSacas;

        //Custo Fixo Mensal

        custoTotal += dados.custoArmazenagem.custoFixoMensal * dados.QuantidadeSacas * meses;

        //Custo percentual anual
        if (dados.custoArmazenagem.CustoPercentualAnual > 0)
        {
            custoTotal = += valorBase * dados.custoArmazenagem.CustoPercentualAnual * ((decimal)dados.PrazoDias / 365m);
        }
        //Custo por Tonelada Mês

        custoTotal += dados.custoArmazenagem.custoPorToneladaMes * toneladas * meses;

        //Seguro
        if (dados.custoArmazenagem.Seguro > 0)
        {
            custoTotal += valorBase * dados.custoArmazenagem.Seguro * ((decimal)dados.PrazoDias / 365m);
        }

        //Manutenção
        custoTotal += dados.custoArmazenagem.Manutencao * dados.QuantidadeSacas * meses;

        _logger.LogDebug($"Custo armazenagem calculado: R$ {Custo}", custoTotal);
        return custoTotal;
    } }
