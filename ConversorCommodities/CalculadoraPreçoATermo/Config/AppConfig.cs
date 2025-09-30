namespace PrecoTermoCalculator.Config;

public class AppConfig
{
    public ApiConfig Api { get; set; } = new();
    public CalculoConfig Calculo { get; set; } = new();
    public LogConfig Log { get; set; } = new();
}

public class ApiConfig
{
    public int TimeoutSegundos { get; set; } = 10;
    public string UserAgent { get; set; } = "PrecoTermoCalculator/2.0";
    public Dictionary<string, string> Endpoints { get; set; } = new()
    {
        ["CDI_BrasilAPI"] = "https://brasilapi.com.br/api/cdi",
        ["SELIC_BrasilAPI"] = "https://brasilapi.com.br/api/selic",
        ["SELIC_BancoCentral"] = "https://api.bcb.gov.br/dados/serie/bcdata.sgs.11/dados/ultimos/1?formato=json"
    };
}

public class CalculoConfig
{
    public int DiasUteisAno { get; set; } = 252;
    public decimal PesoSacaPadrao { get; set; } = 60m;
    public bool UsarCompostoContinuo { get; set; } = false;
}

public class LogConfig
{
    public string Level { get; set; } = "Information";
    public bool SalvarArquivo { get; set; } = true;
    public string DiretorioLogs { get; set; } = "logs";
}