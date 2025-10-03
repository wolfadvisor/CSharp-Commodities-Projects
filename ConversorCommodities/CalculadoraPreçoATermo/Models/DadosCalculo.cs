using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraPreçoATermo.Models
{
    public class DadosCalculo
    {
        public string Commodity { get; set; } = string.Empty;
        public decimal PrecoSpot { get; set; }
        public decimal QuantidadeSacas { get; set; }
        public decimal PesoSacaKg { get; set; } = 60m;
        public int PrazoDias { get; set; }
        public decimal TaxaJuros { get; set; }
        public string FonteTaxa { get; set; } = string.Empty;
        public CustoArmagenazem CustoArmazenagem { get; set; } = new();
        public DateTime DataCalculo { get; set; } = DateTime.Now;
        public string Observacoes { get; set; } = string.Empty;
    }
}
