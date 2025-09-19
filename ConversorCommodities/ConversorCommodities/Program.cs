using ConversorCommodities.Services;

namespace ConversorCommodities
{
    /// <summary>
    /// Programa principal do Conversor de Commodities
    /// Aplicação console para conversão de pesos (sacas/toneladas) e moedas (Real/Dólar)
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Configurar console para UTF-8 (suporte a caracteres especiais)
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Instanciar serviço de cotação com using para garantir dispose
            using var currencyService = new CurrencyService();

            bool continuar = true;

            // Exibir mensagem de boas-vindas
            Console.WriteLine("Bem-vindo ao Conversor de Commodities!");
            Console.WriteLine("Sistema desenvolvido para conversões de peso e moeda.");
            UserInterface.AguardarTecla();

            // Loop principal do programa
            while (continuar)
            {
                try
                {
                    int opcao = UserInterface.ExibirMenuPrincipal();

                    switch (opcao)
                    {
                        case 0:
                            continuar = false;
                            UserInterface.ExibirMensagemSucesso("Obrigado por usar o Conversor de Commodities!");
                            break;

                        case 1:
                            await ProcessarConversaoSacasParaToneladas();
                            break;

                        case 2:
                            await ProcessarConversaoToneladasParaSacas();
                            break;

                        case 3:
                            await ProcessarConversaoQuilosParaToneladas();
                            break;

                        case 4:
                            await ProcessarConversaoToneladasParaQuilos();
                            break;

                        case 5:
                            await ProcessarConversaoRealParaDolar(currencyService);
                            break;

                        case 6:
                            await ProcessarConversaoDolarParaReal(currencyService);
                            break;
                    }

                    if (opcao != 0)
                    {
                        continuar = UserInterface.DesejaNovaConversao();
                    }
                }
                catch (Exception ex)
                {
                    UserInterface.ExibirMensagemErro($"Erro inesperado: {ex.Message}");
                    UserInterface.AguardarTecla();
                }
            }
        }

        /// <summary>
        /// Processa conversão de sacas para toneladas
        /// </summary>
        private static Task ProcessarConversaoSacasParaToneladas()
        {
            try
            {
                double sacas = UserInterface.LerValorDouble("Digite a quantidade de sacas de 60kg: ");
                double toneladas = CommodityConverter.ConverterSacasParaToneladas(sacas);

                UserInterface.ExibirResultadoPeso(sacas, "sacas de 60kg", toneladas, "toneladas");
            }
            catch (ArgumentException ex)
            {
                UserInterface.ExibirMensagemErro(ex.Message);
            }

            UserInterface.AguardarTecla();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Processa conversão de toneladas para sacas
        /// </summary>
        private static Task ProcessarConversaoToneladasParaSacas()
        {
            try
            {
                double toneladas = UserInterface.LerValorDouble("Digite a quantidade em toneladas: ");
                double sacas = CommodityConverter.ConverterToneladasParaSacas(toneladas);

                UserInterface.ExibirResultadoPeso(toneladas, "toneladas", sacas, "sacas de 60kg");
            }
            catch (ArgumentException ex)
            {
                UserInterface.ExibirMensagemErro(ex.Message);
            }

            UserInterface.AguardarTecla();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Processa conversão de quilos para toneladas
        /// </summary>
        private static Task ProcessarConversaoQuilosParaToneladas()
        {
            try
            {
                double quilos = UserInterface.LerValorDouble("Digite a quantidade em quilos: ");
                double toneladas = CommodityConverter.ConverterQuilosParaToneladas(quilos);

                UserInterface.ExibirResultadoPeso(quilos, "quilos", toneladas, "toneladas");
            }
            catch (ArgumentException ex)
            {
                UserInterface.ExibirMensagemErro(ex.Message);
            }

            UserInterface.AguardarTecla();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Processa conversão de toneladas para quilos
        /// </summary>
        private static Task ProcessarConversaoToneladasParaQuilos()
        {
            try
            {
                double toneladas = UserInterface.LerValorDouble("Digite a quantidade em toneladas: ");
                double quilos = CommodityConverter.ConverterToneladasParaQuilos(toneladas);

                UserInterface.ExibirResultadoPeso(toneladas, "toneladas", quilos, "quilos");
            }
            catch (ArgumentException ex)
            {
                UserInterface.ExibirMensagemErro(ex.Message);
            }

            UserInterface.AguardarTecla();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Processa conversão de Real para Dólar
        /// </summary>
        private static async Task ProcessarConversaoRealParaDolar(CurrencyService currencyService)
        {
            try
            {
                decimal valorReais = UserInterface.LerValorDecimal("Digite o valor em reais (R$): ");
                var (valorDolar, cotacao) = await currencyService.ConverterRealParaDolarAsync(valorReais);

                UserInterface.ExibirResultadoMonetario(valorReais, "R$", valorDolar, "US$", cotacao);
            }
            catch (HttpRequestException ex)
            {
                UserInterface.ExibirMensagemErro($"Erro de conexão: {ex.Message}");
                UserInterface.ExibirMensagemErro("Verifique sua conexão com a internet e tente novamente.");
            }
            catch (InvalidOperationException ex)
            {
                UserInterface.ExibirMensagemErro(ex.Message);
                UserInterface.ExibirMensagemErro("A cotação pode não estar disponível nos fins de semana e feriados.");
            }
            catch (Exception ex)
            {
                UserInterface.ExibirMensagemErro($"Erro ao obter cotação: {ex.Message}");
            }

            UserInterface.AguardarTecla();
        }

        /// <summary>
        /// Processa conversão de Dólar para Real
        /// </summary>
        private static async Task ProcessarConversaoDolarParaReal(CurrencyService currencyService)
        {
            try
            {
                decimal valorDolar = UserInterface.LerValorDecimal("Digite o valor em dólares (US$): ");
                var (valorReais, cotacao) = await currencyService.ConverterDolarParaRealAsync(valorDolar);

                UserInterface.ExibirResultadoMonetario(valorDolar, "US$", valorReais, "R$", cotacao);
            }
            catch (HttpRequestException ex)
            {
                UserInterface.ExibirMensagemErro($"Erro de conexão: {ex.Message}");
                UserInterface.ExibirMensagemErro("Verifique sua conexão com a internet e tente novamente.");
            }
            catch (InvalidOperationException ex)
            {
                UserInterface.ExibirMensagemErro(ex.Message);
                UserInterface.ExibirMensagemErro("A cotação pode não estar disponível nos fins de semana e feriados.");
            }
            catch (Exception ex)
            {
                UserInterface.ExibirMensagemErro($"Erro ao obter cotação: {ex.Message}");
            }

            UserInterface.AguardarTecla();
        }
    }
}