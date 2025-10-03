using CalculadoraPreçoATermo.Models;
using Microsoft.Extensions.Logging;


namespace PrecoTermoCalculator.Helpers;

public class DisplayHelper
{
    private readonly ILogger<DisplayHelper> _logger;

    public DisplayHelper(ILogger<DisplayHelper> logger)
    {
        _logger = logger;
    }

    public void ExibirCabecalho()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Clear();

        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine("    CALCULADORA DE PREÇO A TERMO PARA COMMODITIES");
        Console.WriteLine("         Versão Profissional 2.0 - Brasil");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine($"📅 Data: {DateTime.Now:dd/MM/yyyy HH:mm}");
        Console.WriteLine();

        _logger.LogInformation("Interface iniciada em {DataHora}", DateTime.Now);
    }

    public void ExibirResultados(DadosCalculo dados, ResultadoCalculos resultados)
    {
        Console.Clear();
        ExibirRelatorioCompleto(dados, resultados);

        Console.WriteLine("\n" + new string('═', 60));
        Console.WriteLine("💾 OPÇÕES DE EXPORTAÇÃO");
        Console.WriteLine(new string('─', 25));
        Console.WriteLine("1 - Salvar relatório em arquivo texto");
        Console.WriteLine("2 - Exibir resumo executivo");
        Console.WriteLine("3 - Finalizar");

        var opcao = LerOpcao("Escolha uma opção (1-3): ", 1, 3);

        switch (opcao)
        {
            case 1:
                SalvarRelatorio(dados, resultados);
                break;
            case 2:
                ExibirResumoExecutivo(dados, resultados);
                break;
        }
    }
    private void ExibirRelatorioCompleto(DadosCalculo dados, ResultadoCalculos resultado)
    {
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine("                  RELATÓRIO DE RESULTADOS");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine();

        ExibirSecaoResumo(dados, resultado);
        ExibirSecaoPrecos(dados, resultado);
        ExibirSecaoToneladas(resultado); 
    }
    private void ExibirSecaoResumo(DadosCalculo dados, ResultadoCalculos resultado)
    {
        Console.WriteLine("RESUMO DE DADOS:");
        Console.WriteLine(new string('-', 20));
        Console.WriteLine($"Commodity:           {dados.Commodity}");
        Console.WriteLine($"Quantidade:          {dados.QuantidadeSacas:N2} sacas ({resultado.ToneladasTotal:N2} toneladas)");
        Console.WriteLine($"Peso por saca:       {dados.PesoSacaKg:N2} kg");
        Console.WriteLine($"Prazo:               {dados.PrazoDias} dias ({(decimal)dados.PrazoDias / 30:N1} meses)");
        Console.WriteLine($"Taxa de juros:       {dados.TaxaJuros * 100:N4}% a.a.");
        Console.WriteLine($"Fonte da taxa:       {dados.FonteTaxa}");

        if (!string.IsNullOrEmpty(dados.Observacoes))
            Console.WriteLine($"Observações:             {dados.Observacoes}");
        Console.WriteLine();
    }

    private void ExibirSecaoPrecos(DadosCalculo dados, ResultadoCalculos resultado)
    {
        Console.WriteLine("PESO E VALORES:");
        Console.WriteLine(new string('-', 20));
        Console.WriteLine("💰 PREÇOS E VALORES");
        Console.WriteLine(new string('─', 20));
        Console.WriteLine($"Preço spot unitário:       R$ {dados.PrecoSpot:N2}");
        Console.WriteLine($"Preço a termo unitário:    R$ {resultado.PrecoTermoUnitario:N2}");
        Console.WriteLine($"{"Diferença unitária:",-25} R$ {resultado.PrecoTermoUnitario - dados.PrecoSpot:N2} " +
                         $"({((resultado.PrecoTermoUnitario / dados.PrecoSpot) - 1) * 100:+0.00;-0.00}%)");
        Console.WriteLine();

        Console.WriteLine($"Valor spot total:          R$ {resultado.ValorSpotTotal:N2}");
        Console.WriteLine($"Valor a termo total:       R$ {resultado.ValorTermoTotal:N2}");
        Console.WriteLine($"{"Diferença total:",-25} R$ {resultado.DiferencaTotal:N2} " +
                         $"({resultado.PercentualDiferenca:+0.00;-0.00}%)");
        Console.WriteLine();

    }
    private void ExibirSecaoToneladas(ResultadoCalculos resultados)
    {
        Console.WriteLine("VALORES POR TONELADA");
        Console.WriteLine(new string('─', 24));
        Console.WriteLine($"Preço spot por tonelada:     R$ {resultados.PrecoSpotPorTonelada:N2}");
        Console.WriteLine($"Preço a termo por tonelada:  R$ {resultados.PrecoTermoTonelada:N2}");
        Console.WriteLine($"Diferença por tonelada:      R$ {resultados.PrecoTermoTonelada - resultados.PrecoSpotPorTonelada:N2}");
        Console.WriteLine();

    }

    private void ExibirSecaoCustos(DadosCalculo dados, ResultadoCalculos resultados)
    {
        Console.WriteLine("DETALHAMENTO DE CUSTOS");
        Console.WriteLine(new string('─', 26));
        Console.WriteLine($"Custo de armazenagem:      R$ {resultados.CustoArmagenazemTotal:N2}");
        Console.WriteLine($"Custo financeiro (juros):  R$ {resultados.CustoJurosTotal:N2}");
        Console.WriteLine($"{"Custo total:",-25} R$ {resultados.DiferencaTotal:N2}");
        Console.WriteLine();

        Console.WriteLine($"Tipo de armazenagem:       {dados.CustoArmazenagem.Tipo}");

        if (dados.CustoArmazenagem.CustoFixoMensal > 0)
            Console.WriteLine($"  • Custo fixo mensal:     R$ {dados.CustoArmazenagem.CustoFixoMensal:N2}/saca");

        if (dados.CustoArmazenagem.CustoPercentualAnual > 0)
            Console.WriteLine($"  • Custo percentual:      {dados.CustoArmazenagem.CustoPercentualAnual * 100:N2}% a.a.");

        if (dados.CustoArmazenagem.CustoPorToneladaMes > 0)
            Console.WriteLine($"  • Custo por tonelada:    R$ {dados.CustoArmazenagem.CustoPorToneladaMes:N2}/ton/mês");

        if (dados.CustoArmazenagem.Seguro > 0)
            Console.WriteLine($"  • Seguro:                {dados.CustoArmazenagem.Seguro * 100:N2}% a.a.");

        if (dados.CustoArmazenagem.Manutencao > 0)
            Console.WriteLine($"  • Manutenção:            R$ {dados.CustoArmazenagem.Manutencao:N2}/saca/mês");

        Console.WriteLine();
    }
    private void ExibirSecaoAnalise(ResultadoCalculos resultados)
    {
        Console.WriteLine("ANÁLISE FINANCEIRA");
        Console.WriteLine(new string('─', 22));
        Console.WriteLine($"Diferença percentual:      {resultados.PercentualDiferenca:N4}%");
        Console.WriteLine($"Taxa efetiva do período:   {resultados.TaxaEfetivaPeriodo:N4}%");

        var taxaMensal = (decimal)(Math.Pow((double)resultados.TaxaEfetivaPeriodo / 100 + 1, 1.0 / ((double)252 / 30)) - 1) * 100;
        Console.WriteLine($"Taxa equivalente mensal:   {taxaMensal:N4}%");

        // Análise de viabilidade
        Console.WriteLine();
        Console.WriteLine("ANÁLISE DE VIABILIDADE");
        Console.WriteLine(new string('─', 26));

        if (resultados.PercentualDiferenca > 0)
        {
            Console.WriteLine("O preço a termo incorpora adequadamente:");
            Console.WriteLine("   • Custo de oportunidade do capital");
            if (resultados.CustoArmagenazemTotal > 0)
                Console.WriteLine("   • Custos de armazenagem e manutenção");
            Console.WriteLine("   • Risco de variação de preços");
        }
        else
        {
            Console.WriteLine("Atenção: Preço a termo menor que spot");
            Console.WriteLine("   • Verifique os parâmetros inseridos");
            Console.WriteLine("   • Pode indicar contador negativo");
        }

        Console.WriteLine();
    }

    private void ExibirSecaoRodape(DadosCalculo dados)
    {
        Console.WriteLine(new string('─', 60));
        Console.WriteLine($" Relatório gerado em: {dados.DataCalculo:dd/MM/yyyy HH:mm:ss}");
        Console.WriteLine($"Sistema: Calculadora de Preço a Termo v2.0");
        Console.WriteLine($"Padrão: Mercado brasileiro (252 dias úteis/ano)");
        Console.WriteLine(new string('═', 60));
    }
    private void ExibirResumoExecutivo(DadosCalculo dados, ResultadoCalculos resultados)
    {
        Console.Clear();
        Console.WriteLine(" RESUMO EXECUTIVO");
        Console.WriteLine(new string('═', 40));
        Console.WriteLine();

        Console.WriteLine($" OPERAÇÃO: {dados.Commodity.ToUpper()}");
        Console.WriteLine($"   Volume: {dados.QuantidadeSacas:N0} sacas ({resultados.ToneladasTotal:N1} MT)");
        Console.WriteLine($"   Prazo: {dados.PrazoDias} dias");
        Console.WriteLine();

        Console.WriteLine($" PREÇOS:");
        Console.WriteLine($"   Spot:   R$ {dados.PrecoSpot:N2} /saca");
        Console.WriteLine($"   Termo:  R$ {resultados.PrecoTermoUnitario:N2} /saca (+{((resultados.PrecoTermoUnitario / dados.PrecoSpot) - 1) * 100:N2}%)");
        Console.WriteLine();

        Console.WriteLine($" VALORES TOTAIS:");
        Console.WriteLine($"   Hoje:   R$ {resultados.ValorSpotTotal:N0}");
        Console.WriteLine($"   Termo:  R$ {resultados.ValorTermoTotal:N0}");
        Console.WriteLine($"   Delta:  R$ {resultados.DiferencaTotal:N0} ({resultados.PercentualDiferenca:+N2;-N2}%)");
        Console.WriteLine();

        if (resultados.CustoArmagenazemTotal > 0)
        {
            Console.WriteLine($"CUSTOS:");
            Console.WriteLine($"   Armazenagem: R$ {resultados.CustoArmagenazemTotal:N0}");
            Console.WriteLine($"   Financeiro:  R$ {resultados.CustoJurosTotal:N0}");
            Console.WriteLine();
        }

        Console.WriteLine($"RECOMENDAÇÃO:");
        if (resultados.PercentualDiferenca > 1)
            Console.WriteLine("    Preço a termo adequadamente precificado");
        else if (resultados.PercentualDiferenca > 0)
            Console.WriteLine("Preço a termo com margem justa");
        else
            Console.WriteLine("Preço a termo pode estar sub precificado");

        Console.WriteLine();
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private void SalvarRelatorio(DadosCalculo dados, ResultadoCalculos resultados)
    {
        try
        {
            var nomeArquivo = $"relatorio_preco_termo_{dados.Commodity.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.txt";

            using var writer = new StreamWriter(nomeArquivo, false, System.Text.Encoding.UTF8);

            writer.WriteLine("RELATÓRIO DE PREÇO A TERMO - COMMODITIES");
            writer.WriteLine(new string('=', 50));
            writer.WriteLine($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            writer.WriteLine();

            writer.WriteLine("DADOS DA OPERAÇÃO:");
            writer.WriteLine($"Commodity: {dados.Commodity}");
            writer.WriteLine($"Quantidade: {dados.QuantidadeSacas:N2} sacas ({resultados.ToneladasTotal:N2} ton)");
            writer.WriteLine($"Preço spot: R$ {dados.PrecoSpot:N2}/saca");
            writer.WriteLine($"Prazo: {dados.PrazoDias} dias");
            writer.WriteLine($"Taxa: {dados.TaxaJuros * 100:N4}% a.a. ({dados.FonteTaxa})");
            writer.WriteLine();

            writer.WriteLine("RESULTADOS:");
            writer.WriteLine($"Preço a termo: R$ {resultados.PrecoTermoUnitario:N2}/saca");
            writer.WriteLine($"Valor total: R$ {resultados.ValorTermoTotal:N2}");
            writer.WriteLine($"Diferença: R$ {resultados.DiferencaTotal:N2} ({resultados.PercentualDiferenca:N4}%)");
            writer.WriteLine();

            if (resultados.CustoArmagenazemTotal > 0)
            {
                writer.WriteLine("CUSTOS:");
                writer.WriteLine($"Armazenagem: R$ {resultados.CustoArmagenazemTotal:N2}");
                writer.WriteLine($"Financeiro: R$ {resultados.CustoJurosTotal:N2}");
                writer.WriteLine();
            }

            if (!string.IsNullOrEmpty(dados.Observacoes))
            {
                writer.WriteLine($"OBSERVAÇÕES: {dados.Observacoes}");
                writer.WriteLine();
            }

            writer.WriteLine(new string('=', 50));
            writer.WriteLine("Sistema: Calculadora de Preço a Termo v2.0");

            Console.WriteLine($"✅ Relatório salvo como: {nomeArquivo}");
            _logger.LogInformation("Relatório salvo: {NomeArquivo}", nomeArquivo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao salvar relatório: {ex.Message}");
            _logger.LogError(ex, "Erro ao salvar relatório");
        }

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private int LerOpcao(string mensagem, int min, int max)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out var opcao) && opcao >= min && opcao <= max)
                return opcao;

            Console.WriteLine($"Digite um número entre {min} e {max}.");
        }
    }
}




