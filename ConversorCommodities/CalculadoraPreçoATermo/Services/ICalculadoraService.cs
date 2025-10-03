using CalculadoraPreçoATermo.Models;

namespace PrecoTermoCalculator.Services;

public interface ICalculadoraService
{
    ResultadoCalculos CalcularPrecoTermo(DadosCalculo dados);
    decimal CalcularCustoArmazenagem(DadosCalculo dados);
}