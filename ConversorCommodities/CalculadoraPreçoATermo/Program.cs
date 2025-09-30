using System;

class PrecoTermo
{
    static void Main()
    {
        Console.WriteLine("=== Calculadora de Preço a Termo ===");

        Console.Write("Digite o preço spot (R$): ");
        decimal spot = Convert.ToDecimal(Console.ReadLine());

        Console.WriteLine("Digite a taxa de juros (% ao ano): ");
        decimal taxa = Convert.ToDecimal(Console.ReadLine());

        Console.WriteLine("Digite o prazo (dias): ");
        int dias = Convert.ToInt32(Console.ReadLine());

        // Formula basica: preco a termo = spot * (1+ taxa*(dias/252))
        decimal termo = spot * (1 + taxa*(dias / 252));

        Console.WriteLine($"Preço a termo esperado: R$ {termo:F2}");

    }
}