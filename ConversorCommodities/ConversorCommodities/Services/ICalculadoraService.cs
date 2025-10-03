using CalculadoraPre�oATermo.Models;

namespace PrecoTermoCalculator.Services;

public interface ICalculadoraService
{
    // ? Corre��o: o retorno agora � "ResultadoCalculos" (plural)
    // ? Antes: havia inconsist�ncia porque o m�todo usava ResultadoCalculos, 
    // mas a classe estava declarada como ResultadoCalculo (singular).
    ResultadoCalculos CalcularPrecoTermo(DadosCalculo dados);

    decimal CalcularCustoArmazenagem(DadosCalculo dados);
}
