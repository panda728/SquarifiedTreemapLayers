using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SquarifiedTreeMapCoordinator;
using SquarifiedTreeMapForge;
using SquarifiedTreeMapForge.Layout;
using SquarifiedTreeMapForge.WinForms;
using SquarifiedTreeMapShared;

namespace SquarifiedTreeMapWinForms;

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

                // SquarifiedTreeMapForge.WinForms.csproj
                services.AddTransient<TreeMapGdiDriver<PivotDataSource>>();
                services.AddTransient<GdiRenderer>();

                // SquarifiedTreeMapForge.csproj
                services.AddTransient<LayoutCoordinator<PivotDataSource>>();
                services.AddTransient<LayoutGenerator<PivotDataSource>>();
                services.AddTransient<AggregatePreparer<PivotDataSource>>();
                services.AddTransient<LegendCalculator>();
                services.AddTransient<ITreemapGenerator, SquarifiedTreemapGenerator>();
            });
}