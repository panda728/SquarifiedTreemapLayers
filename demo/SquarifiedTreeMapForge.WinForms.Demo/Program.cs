using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PopulationDataProvider.Demo;
using SquarifiedTreeMapCoordinator;
using SquarifiedTreeMapForge.Layout;
using SquarifiedTreeMapShared;

namespace SquarifiedTreeMapForge.WinForms.Demo;

internal static class Program
{
    [STAThread]
    static void Main()
    {
#if NET6_0_OR_GREATER
        ApplicationConfiguration.Initialize();
#else
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
        var host = CreateHostBuilder().Build();
        Application.Run(host.Services.GetRequiredService<FormMain>());
    }

    public static IHostBuilder CreateHostBuilder(string[]? args = null) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<TreeMapSettings>(
                    hostContext.Configuration.GetSection("TreeMapSettings"));
                services.Configure<TreeMapLayoutSettings>(
                    hostContext.Configuration.GetSection("TreeMapLayoutSettings"));
                services.Configure<LegendSettings>(
                    hostContext.Configuration.GetSection("LegendSettings"));

                // Settings
                services.AddTransient<TreeMapSettings>();
                services.AddTransient<TreeMapLayoutSettings>();
                services.AddTransient<LegendSettings>();

                // this project
                services.AddTransient<FormMain>();
                services.AddTransient<PopulationDataProvider.Demo.PopulationDataProvider>();

                // SquarifiedTreeMapForge.WinForms.csproj
                services.AddTransient<TreeMapGdiDriver<PopulationData>>();
                services.AddTransient<GdiRenderer>();

                // SquarifiedTreeMapForge.csproj
                services.AddTransient<LayoutCoordinator<PopulationData>>();
                services.AddTransient<LayoutGenerator<PopulationData>>();
                services.AddTransient<AggregatePreparer<PopulationData>>();
                services.AddTransient<LegendCalculator>();
                services.AddTransient<ITreemapGenerator, SquarifiedTreemapGenerator>();
            });
}