using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraPreçoATermo.Models
{
    internal class ResultadoCalculos
    {
        public decimal ToneladasTotal { get; set; }
        public decimal ValorSpotTotal { get; set; }
        public decimal PrecoTermoUnitario { get; set; }
        public decimal ValorTermoTotal { get; set; }
        public decimal PrecoSpotPorTonelada { get; set; }
        public decimal PrecoTermoTonelada { get; set; }
        public decimal CustoArmagenazemTotal { get; set; }
        public decimal CustoJurosTotal { get; set; }
        public decimal DiferencaTotal { get; set; }

        public decimal PercentualDiferenca => ValorSpotTotal > 0 ? (DiferencaTotal / ValorSpotTotal) * 100m : 0m;
        public decimal TaxaEfetivaperiodo => PrecoSpot > 0 ? ((PrecoTermoUnitario / PrecoSpot) - 1) * 100m : 0m;


        private decimal PrecoSpot { get; set; }

        public void SetPrecoSpot(decimal precoSpot) => PrecoSpot = precoSpot;


    }
}
