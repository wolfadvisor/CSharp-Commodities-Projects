using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraPreçoATermo.Models
{
    public class CustoArmagenazem
    {
        public string Tipo { get; set; } = "Nenhum";
        public decimal CustoFixoMensal { get; set; }
        public decimal CustoPercentualAnual { get; set; }
        public decimal CustoPorToneladaMes { get; set; }
        public decimal Seguro { get; set; }
        public decimal Manutencao { get; set; }

        public bool TemCustos => CustoFixoMensal > 0 || CustoPercentualAnual > 0 || CustoPorToneladaMes > 0 || Manutencao > 0;
    }
}
