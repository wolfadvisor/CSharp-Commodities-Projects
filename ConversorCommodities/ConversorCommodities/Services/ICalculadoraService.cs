using CalculadoraPreçoATermo.Models;

namespace PrecoTermoCalculator.Services;

public interface ICalculadoraService
{
    // ? Correção: o retorno agora é "ResultadoCalculos" (plural)
    // ? Antes: havia inconsistência porque o método usava ResultadoCalculos, 
    // mas a classe estava declarada como ResultadoCalculo (singular).
    ResultadoCalculos CalcularPrecoTermo(DadosCalculo dados);

    decimal CalcularCustoArmazenagem(DadosCalculo dados);
}
