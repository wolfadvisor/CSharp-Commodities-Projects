using CalculadoraPreçoATermo.Models;
using CalculadoraPreçoATermo.Services;
using Microsoft.Extensions.Logging;

namespace PrecoTermoCalculator.Helpers;

public class InputHelper
{
    private readonly ILogger<InputHelper> _logger;

    public InputHelper(ILogger<InputHelper> logger)
    {
        _logger = logger;
    }

    public async Task<DadosCalculo> ColetarDadosAsync(IApiService apiService)
    {
        var dados = new DadosCalculo();

        ColetarDadosCommodity(dados);
        ColetarQuantidadePeso(dados);
        ColetarPrazo(dados);
        await ColetarTaxaJurosAsync(dados, apiService);
        await ColetarCustosArmazenagemAsync(dados);

        return dados;
    }

    private void ColetarDadosCommodity(DadosCalculo dados)
    {
        Console.WriteLine("📊 DADOS DA COMMODITY");
        Console.WriteLine("─────────────────────");
        dados.Commodity = LerString("Nome da commodity (ex: Soja, Milho, Café): ", "Soja");
        dados.PrecoSpot = LerDecimal("Preço spot (R$ por saca): ");
        Console.WriteLine();
    }

    private void ColetarQuantidadePeso(DadosCalculo dados)
    {
        Console.WriteLine("📦 QUANTIDADE E PESO");
        Console.WriteLine("────────────────────");
        dados.QuantidadeSacas = LerDecimal("Quantidade (sacas): ");
        dados.PesoSacaKg = LerDecimal("Peso por saca (kg): ", 60m);

        var toneladas = dados.QuantidadeSacas * dados.PesoSacaKg / 1000m;
        Console.WriteLine($"💡 Total: {toneladas:N2} toneladas");
        Console.WriteLine();
    }

    private void ColetarPrazo(DadosCalculo dados)
    {
        Console.WriteLine("📅 PRAZO DO CONTRATO");
        Console.WriteLine("────────────────────");
        dados.PrazoDias = LerInteiro("Prazo (dias): ");

        var meses = dados.PrazoDias / 30.0;
        Console.WriteLine($"💡 Equivale a aproximadamente {meses:N1} meses");
        Console.WriteLine();
    }

    private async Task ColetarTaxaJurosAsync(DadosCalculo dados, IApiService apiService)
    {
        Console.WriteLine("💰 TAXA DE JUROS");
        Console.WriteLine("────────────────");
        Console.WriteLine("1 - Usar taxa CDI atual (API BrasilAPI)");
        Console.WriteLine("2 - Usar taxa SELIC atual (API Banco Central)");
        Console.WriteLine("3 - Inserir taxa manualmente");

        var opcao = LerInteiro("Escolha uma opção (1-3): ", 1, 3);

        try
        {
            switch (opcao)
            {
                case 1:
                    var (sucessoCdi, taxaCdi, fonteCdi) = await apiService.ObterTaxaAsync(TipoTaxa.CDI);
                    if (sucessoCdi)
                    {
                        dados.TaxaJuros = taxaCdi;
                        dados.FonteTaxa = fonteCdi;
                        Console.WriteLine($"✅ Taxa obtida: {taxaCdi * 100:F4}% a.a.");
                    }
                    else
                    {
                        throw new InvalidOperationException("Falha ao obter CDI");
                    }
                    break;

                case 2:
                    var (sucessoSelic, taxaSelic, fonteSelic) = await apiService.ObterTaxaAsync(TipoTaxa.Selic);
                    if (sucessoSelic)
                    {
                        dados.TaxaJuros = taxaSelic;
                        dados.FonteTaxa = fonteSelic;
                        Console.WriteLine($"✅ Taxa obtida: {taxaSelic * 100:F4}% a.a.");
                    }
                    else
                    {
                        throw new InvalidOperationException("Falha ao obter SELIC");
                    }
                    break;

                case 3:
                    dados.TaxaJuros = LerDecimal("Taxa de juros (% ao ano): ") / 100m;
                    dados.FonteTaxa = "Manual";
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao obter taxa via API");
            Console.WriteLine($"⚠️  Erro ao obter taxa via API: {ex.Message}");
            Console.WriteLine("Utilizando entrada manual...");
            dados.TaxaJuros = LerDecimal("Taxa de juros (% ao ano): ") / 100m;
            dados.FonteTaxa = "Manual (fallback)";
        }
        Console.WriteLine();
    }

    private async Task ColetarCustosArmazenagemAsync(DadosCalculo dados)
    {
        Console.WriteLine("🏢 CUSTOS DE ARMAZENAGEM");
        Console.WriteLine("───────────────────────");

        if (!ConfirmarAcao("Incluir custos de armazenagem? (s/n): "))
        {
            Console.WriteLine("ℹ️  Custos de armazenagem não incluídos.");
            Console.WriteLine();
            return;
        }

        Console.WriteLine("\nTipos de custo disponíveis:");
        Console.WriteLine("1 - Custo fixo mensal (R$ por saca/mês)");
        Console.WriteLine("2 - Custo percentual anual (% do valor)");
        Console.WriteLine("3 - Custo por tonelada/mês");
        Console.WriteLine("4 - Configuração personalizada");

        var tipoCusto = LerInteiro("Escolha o tipo (1-4): ", 1, 4);

        switch (tipoCusto)
        {
            case 1:
                dados.CustoArmazenagem.CustoFixoMensal = LerDecimal("Custo mensal por saca (R$): ");
                dados.CustoArmazenagem.Tipo = "Fixo Mensal";
                break;

            case 2:
                dados.CustoArmazenagem.CustoPercentualAnual = LerDecimal("Custo percentual anual (%): ") / 100m;
                dados.CustoArmazenagem.Tipo = "Percentual Anual";
                break;

            case 3:
                dados.CustoArmazenagem.CustoPorToneladaMes = LerDecimal("Custo por tonelada/mês (R$): ");
                dados.CustoArmazenagem.Tipo = "Por Tonelada/Mês";
                break;

            case 4:
                Console.WriteLine("\n🔧 CONFIGURAÇÃO PERSONALIZADA");
                dados.CustoArmazenagem.CustoFixoMensal = LerDecimal("Custo fixo mensal por saca (R$): ", 0m);
                dados.CustoArmazenagem.CustoPercentualAnual = LerDecimal("Custo percentual anual (%): ", 0m) / 100m;
                dados.CustoArmazenagem.CustoPorToneladaMes = LerDecimal("Custo adicional por ton/mês (R$): ", 0m);
                dados.CustoArmazenagem.Tipo = "Personalizado";
                break;
        }

        // Custos adicionais opcionais
        if (ConfirmarAcao("\nIncluir custos adicionais (seguro, manutenção)? (s/n): "))
        {
            Console.WriteLine("\n💼 CUSTOS ADICIONAIS");
            dados.CustoArmazenagem.Seguro = LerDecimal("Seguro (% do valor ao ano): ", 0m) / 100m;
            dados.CustoArmazenagem.Manutencao = LerDecimal("Manutenção mensal por saca (R$): ", 0m);
        }

        // Observações opcionais
        Console.Write("Observações (opcional): ");
        dados.Observacoes = Console.ReadLine() ?? string.Empty;

        Console.WriteLine();
    }

    private string LerString(string mensagem, string valorPadrao = "")
    {
        Console.Write(mensagem);
        if (!string.IsNullOrEmpty(valorPadrao))
            Console.Write($"[padrão: {valorPadrao}] ");

        var input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? valorPadrao : input.Trim();
    }

    private decimal LerDecimal(string mensagem, decimal? valorPadrao = null)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (valorPadrao.HasValue)
                Console.Write($"[padrão: {valorPadrao}] ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) && valorPadrao.HasValue)
                return valorPadrao.Value;

            if (decimal.TryParse(input, out var result) && result >= 0)
                return result;

            Console.WriteLine("⚠️  Por favor, digite um número válido e não negativo.");
        }
    }

    private int LerInteiro(string mensagem, int min = 1, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out var result) && result >= min && result <= max)
                return result;

            Console.WriteLine($"⚠️  Por favor, digite um número entre {min} e {max}.");
        }
    }

    private bool ConfirmarAcao(string mensagem)
    {
        Console.Write(mensagem);
        var resposta = Console.ReadLine()?.ToLower().Trim();
        return resposta == "s" || resposta == "sim" || resposta == "y" || resposta == "yes";
    }
}
