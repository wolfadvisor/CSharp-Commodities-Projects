using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraPreçoATermo.Services
{
    internal class IApiService
    {
        Task<decimal> ObterCDIAsync();
        Task<decimal> ObterSelicAsync();
        Task<(bool sucesso, decimal taxa, string fonte)> ObterTaxaAsync(TipoTaxa tipo);
    }

    public enum TipoTaxa
    {
        CDI,
        Selic
    }
}
