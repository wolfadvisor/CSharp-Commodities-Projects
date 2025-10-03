namespace CalculadoraPreçoATermo.Models
{
    // ? Correção: Nome padronizado como "ResultadoCalculos" (plural)
    // ? Antes: a classe estava como "ResultadoCalculo" (singular),
    // enquanto a interface e service usavam "ResultadoCalculos".
    public class ResultadoCalculos
    {
        public decimal ToneladasTotal { get; set; }
        public decimal ValorSpotTotal { get; set; }
        public decimal PrecoSpotPorTonelada { get; set; }
        public decimal PrecoTermoUnitario { get; set; }
        public decimal ValorTermoTotal { get; set; }
        public decimal PrecoTermoTonelada { get; set; }

        // ? Correção: propriedade corretamente nomeada
        // ? Antes: erro de digitação em "CustoArmagenazemTotal"
        public decimal CustoArmazenagemTotal { get; set; }

        public decimal CustoJurosTotal { get; set; }
        public decimal DiferencaTotal { get; set; }

        private decimal _precoSpot;
        public void SetPrecoSpot(decimal precoSpot)
        {
            _precoSpot = precoSpot;
        }
    }
}
