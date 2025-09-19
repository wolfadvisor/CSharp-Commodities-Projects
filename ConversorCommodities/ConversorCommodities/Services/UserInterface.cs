namespace ConversorCommodities.Services
{
    /// <summary>
    /// Classe responsável pela interface com o usuário
    /// </summary>
    public static class UserInterface
    {
        /// <summary>
        /// Exibe o menu principal e retorna a opção escolhida
        /// </summary>
        public static int ExibirMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║     CONVERSOR DE COMMODITIES         ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║                                      ║");
            Console.WriteLine("║  CONVERSÕES DE PESO:                 ║");
            Console.WriteLine("║  1. Sacas de 60kg → Toneladas        ║");
            Console.WriteLine("║  2. Toneladas → Sacas de 60kg        ║");
            Console.WriteLine("║  3. Quilos → Toneladas               ║");
            Console.WriteLine("║  4. Toneladas → Quilos               ║");
            Console.WriteLine("║                                      ║");
            Console.WriteLine("║  CONVERSÕES MONETÁRIAS:              ║");
            Console.WriteLine("║  5. Real → Dólar (USD)               ║");
            Console.WriteLine("║  6. Dólar (USD) → Real               ║");
            Console.WriteLine("║                                      ║");
            Console.WriteLine("║  0. Sair                             ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();

            return LerOpcaoValida(0, 6);
        }

        /// <summary>
        /// Lê um número válido dentro do intervalo especificado
        /// </summary>
        public static int LerOpcaoValida(int min, int max)
        {
            int opcao;
            do
            {
                Console.Write($"Digite sua opção ({min}-{max}): ");
                if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= min && opcao <= max)
                {
                    break;
                }
                ExibirMensagemErro($"Opção inválida! Digite um número entre {min} e {max}.");
            } while (true);

            return opcao;
        }

        /// <summary>
        /// Lê um valor decimal válido
        /// </summary>
        public static decimal LerValorDecimal(string mensagem)
        {
            decimal valor;
            do
            {
                Console.Write(mensagem);
                if (decimal.TryParse(Console.ReadLine()?.Replace(",", "."),
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out valor) && valor >= 0)
                {
                    break;
                }
                ExibirMensagemErro("Valor inválido! Digite um número positivo.");
            } while (true);

            return valor;
        }

        /// <summary>
        /// Lê um valor double válido
        /// </summary>
        public static double LerValorDouble(string mensagem)
        {
            double valor;
            do
            {
                Console.Write(mensagem);
                if (double.TryParse(Console.ReadLine()?.Replace(",", "."),
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out valor) && valor >= 0)
                {
                    break;
                }
                ExibirMensagemErro("Valor inválido! Digite um número positivo.");
            } while (true);

            return valor;
        }

        /// <summary>
        /// Exibe resultado de conversão de peso
        /// </summary>
        public static void ExibirResultadoPeso(double valorOriginal, string unidadeOriginal,
                                              double valorConvertido, string unidadeConvertida)
        {
            Console.WriteLine();
            Console.WriteLine("═══ RESULTADO DA CONVERSÃO ═══");
            Console.WriteLine($"{valorOriginal:N2} {unidadeOriginal} = {valorConvertido:N2} {unidadeConvertida}");
            Console.WriteLine("════════════════════════════════");
        }

        /// <summary>
        /// Exibe resultado de conversão monetária
        /// </summary>
        public static void ExibirResultadoMonetario(decimal valorOriginal, string moedaOriginal,
                                                   decimal valorConvertido, string moedaConvertida,
                                                   decimal cotacao)
        {
            Console.WriteLine();
            Console.WriteLine("═══ RESULTADO DA CONVERSÃO ═══");
            Console.WriteLine($"{moedaOriginal} {valorOriginal:N2} = {moedaConvertida} {valorConvertido:N2}");
            Console.WriteLine($"Cotação utilizada: R$ {cotacao:N4}");
            Console.WriteLine($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}");
            Console.WriteLine("════════════════════════════════");
        }

        /// <summary>
        /// Exibe mensagem de erro
        /// </summary>
        public static void ExibirMensagemErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {mensagem}");
            Console.ResetColor();
        }

        /// <summary>
        /// Exibe mensagem de sucesso
        /// </summary>
        public static void ExibirMensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ {mensagem}");
            Console.ResetColor();
        }

        /// <summary>
        /// Aguarda o usuário pressionar uma tecla
        /// </summary>
        public static void AguardarTecla()
        {
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Pergunta se o usuário deseja fazer nova conversão
        /// </summary>
        public static bool DesejaNovaConversao()
        {
            Console.WriteLine();
            Console.Write("Deseja fazer uma nova conversão? (S/N): ");
            var resposta = Console.ReadLine()?.ToUpper().Trim();
            return resposta == "S" || resposta == "SIM";
        }
    }
}