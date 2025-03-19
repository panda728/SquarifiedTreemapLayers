using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PopulationDataProvider.Demo;
using SquarifiedTreemapInteractor;
using SquarifiedTreemapForge.Layout;
using SquarifiedTreemapShared;

namespace SquarifiedTreemapForge.WinForms.Demo;

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
                services.Configure<TreemapSettings>(
                    hostContext.Configuration.GetSection("TreemapSettings"));
                services.Configure<TreemapLayoutSettings>(
                    hostContext.Configuration.GetSection("TreemapLayoutSettings"));
                services.Configure<LegendSettings>(
                    hostContext.Configuration.GetSection("LegendSettings"));

                // Settings
                services.AddTransient<TreemapSettings>();
                services.AddTransient<TreemapLayoutSettings>();
                services.AddTransient<LegendSettings>();

                // this project
                services.AddTransient<FormMain>();
                services.AddTransient<PopulationDataProvider.Demo.PopulationDataProvider>();

                // SquarifiedTreemapForge.WinForms.csproj
                services.AddTransient<TreemapGdiDriver<PopulationData>>();
                services.AddTransient<GdiRenderer>();

                // SquarifiedTreemapForge.csproj
                services.AddTransient<LayoutInteractor<PopulationData>>();
                services.AddTransient<LayoutGenerator<PopulationData>>();
                services.AddTransient<DataGroupPreparer<PopulationData>>();
                services.AddTransient<LegendCalculator>();
                services.AddTransient<ITreemapGenerator, SquarifiedTreemapGenerator>();
            });
}