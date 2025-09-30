namespace CalculadoraPreçoATermo.Helpers;

public class InputHelper
{
    private readonly Ilogger<InputHelper> _logger;

    public InputHelper(Ilogger<InputHelper> Logger)
    {
        _logger = Logger;

    }

    public async Task<DadosCalculo> ColetarDadosAsync(IApiService apiService)
    {
        var dados = new DadosCalculo();

        ColetarDadosCommodity(dados);
        ColetarQuantidadePeso(dados);
        ColetarPrazos(dados);
        await ColetarTaxaJurosAsync(dados, apiService);
        await ColetarCustaisArmagenagemAsync(dados);

        return dados;

    }

    private void ColetarDadosCommodity(DadosCalculo dados)
    {
        Console.WriteLine("DADOS DA COMMODITY:");
        Console.WriteLine("-------------------");
        dados.Commodity = LerString("Nome da Commodity (ex: Soja, Milho, Café): ");
        dados.PrecoSpot = LerDecimal("Preço spot (R$ por saca): ");
        Console.WriteLine();
    }
    private void ColetarQuantidadePeso(DadosCalculo dados)
    {
        Console.WriteLine(" QUANTIDADE E PESO ");
        Console.WriteLine("-------------------");
        dados.QuantidadeSacas = lerDecimal("Quantidade de sacas: ");
        dados.PesoSacasKg = LerDecimal("Peso por saca em kg: ");
        Console.WriteLine();

        var toneladas = dados.QuantidadeSacas * dados.PesoSacasKg / 1000;
        Console.WriteLine($"Total {toneladas:N2} toneladas");
        Console.WriteLine();

    }
    private void ColetarPrazo(DadosCalculo dados)
    {
        Console.WriteLine("Prazo do Contrato");
        Console.WriteLine("-------------------");
        dados.PrazoDias = lerInteiro("Prazo em Dias: ");
        
        var meses = dados.PrazoDias / 30;

        Console.WriteLine($"Equivale aproximadamente a {meses:N2} meses");
        Console.WriteLine();
    }

    private async Task ColetarTaxasJuros(DadosCalculos dados)
    {
        Console.WriteLine("TAXA DE JUROS");
        Console.WriteLine("────────────────");
        Console.WriteLine("1 - Usar taxa CDI atual (API BrasilAPI)");
        Console.WriteLine("2 - Usar taxa SELIC atual (API Banco Central)");
        Console.WriteLine("3 - Inserir taxa manualmente");

        var opcao = LerInteiro("Escolha uma opção (1-3): ", 1, 3);

        try
        {
            switch(opcao)
            {
                case 1:
                    var() = await 
            }
        }
    }
}
