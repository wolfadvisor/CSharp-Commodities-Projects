namespace ConversorCommodities.Services
{
    /// <summary>
    /// Classe para conversões de commodities (milho, soja, etc.)
    /// </summary>
    public static class CommodityConverter
    {
        // Constantes para facilitar manutenção e legibilidade
        public const double PESO_SACA_KG = 60.0;
        public const double KG_POR_TONELADA = 1000.0;

        /// <summary>
        /// Converte sacas de 60kg para toneladas
        /// </summary>
        /// <param name="sacas">Quantidade de sacas</param>
        /// <returns>Peso em toneladas</returns>
        public static double ConverterSacasParaToneladas(double sacas)
        {
            if (sacas < 0)
                throw new ArgumentException("Quantidade de sacas não pode ser negativa", nameof(sacas));

            // Cálculo: sacas * 60kg / 1000kg/ton
            return (sacas * PESO_SACA_KG) / KG_POR_TONELADA;
        }

        /// <summary>
        /// Converte toneladas para sacas de 60kg
        /// </summary>
        /// <param name="toneladas">Peso em toneladas</param>
        /// <returns>Quantidade de sacas</returns>
        public static double ConverterToneladasParaSacas(double toneladas)
        {
            if (toneladas < 0)
                throw new ArgumentException("Peso em toneladas não pode ser negativo", nameof(toneladas));

            // Cálculo: toneladas * 1000kg/ton / 60kg/saca
            return (toneladas * KG_POR_TONELADA) / PESO_SACA_KG;
        }

        /// <summary>
        /// Converte quilos diretamente para toneladas
        /// </summary>
        public static double ConverterQuilosParaToneladas(double quilos)
        {
            if (quilos < 0)
                throw new ArgumentException("Peso em quilos não pode ser negativo", nameof(quilos));

            return quilos / KG_POR_TONELADA;
        }

        /// <summary>
        /// Converte toneladas para quilos
        /// </summary>
        public static double ConverterToneladasParaQuilos(double toneladas)
        {
            if (toneladas < 0)
                throw new ArgumentException("Peso em toneladas não pode ser negativo", nameof(toneladas));

            return toneladas * KG_POR_TONELADA;
        }
    }
}