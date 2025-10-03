using CalculadoraPreçoATermo.Models;

namespace PrecoTermoCalculator.Services
{
    public interface ICalculadoraService
    {
        ResultadoCalculo CalcularPrecoTermo(DadosCalculo dados);
        decimal CalcularCustoArmazenagem(DadosCalculo dados);
    }
}
