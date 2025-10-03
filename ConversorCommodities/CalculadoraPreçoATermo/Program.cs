using CalculadoraPreçoATermo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrecoTermoCalculator.Helpers;
using PrecoTermoCalculator.Services;
using Microsoft.Extensions.Http;

namespace PrecoTermoCalculator;

public class Program
{
    private static ServiceProvider? _serviceProvider;

    public static async Task Main(string[] args)
    {
        ConfigureServices();

        if (_serviceProvider == null)
        {
            Console.WriteLine("Erro interno: ServiceProvider não foi inicializado.");
            return;
        }

        var logger = _serviceProvider.GetRequiredService<ILogger<Program>>();
        var displayHelper = _serviceProvider.GetRequiredService<DisplayHelper>();

        try
        {
            logger.LogInformation("Iniciando Calculadora de Preço a Termo.");
            displayHelper.ExibirCabecalho();

            var inputHelper = _serviceProvider.GetRequiredService<InputHelper>();
            var apiService = _serviceProvider.GetRequiredService<IApiService>();
            var calculadoraService = _serviceProvider.GetRequiredService<ICalculadoraService>();

            var dados = await inputHelper.ColetarDadosAsync(apiService);
            var resultados = calculadoraService.CalcularPrecoTermo(dados);

            displayHelper.ExibirResultados(dados, resultados);
            logger.LogInformation("Cálculo concluído com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro durante a execução da aplicação");
            Console.WriteLine($"Erro na aplicação: {ex.Message}");
        }
        finally
        {
            DisposeServices();
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }

    private static void ConfigureServices()
    {
        var services = new ServiceCollection();

        // Logging
        services.AddLogging(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Information);
        });

        // HttpClient para ApiService
        object value = services.AddHttpClient<IApiService, ApiService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("User-Agent", "PrecoTermoCalculator/1.2.0");
        });

        // Services
        services.AddScoped<ICalculadoraService, CalculadoraService>();

        // Helpers
        services.AddScoped<InputHelper>();
        services.AddScoped<DisplayHelper>();

        _serviceProvider = services.BuildServiceProvider();
    }

    private static void DisposeServices()
    {
        _serviceProvider?.Dispose();
    }
}
