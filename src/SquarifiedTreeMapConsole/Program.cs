using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SquarifiedTreemapForge;
using SquarifiedTreemapConsole;
using SquarifiedTreemapForge.Layout;
using SquarifiedTreemapForge.WinForms;
using SquarifiedTreemapShared;

class Program
{
    /// <example>SquarifiedTreemapConsole.exe 1920 1080 data.json treemap.png</example>
    /// <example>SquarifiedTreemapConsole.exe 1920 1080 data.json</example>
    static void Main(string[] args)
    {
        if (!ParseArguments(args, out var width, out var height, out var dataPath, out var pngPath))
        {
            return;
        }
        var host = CreateHostBuilder(args).Build();
        var exporter = host.Services.GetRequiredService<Exporter>();
        var result = exporter.Export(width, height, dataPath, pngPath);
        Console.WriteLine(result);
    }

    static bool ParseArguments(string[] args, out int width, out int height, out string dataPath, out string? pngPath)
    {
        width = 0;
        height = 0;
        dataPath = "";
        pngPath = "";

        if (args.Length < 3)
        {
            Console.WriteLine("Usage: <width> <height> <datapath> [pngfullpath]");
            return false;
        }

        dataPath = args[2];
        if (!int.TryParse(args[0], out width) || !int.TryParse(args[1], out height))
        {
            Console.WriteLine("Width and height must be integers.");
            return false;
        }

        pngPath = args.Length > 3 ? args[3] : null;
        return true;
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
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
                services.AddTransient<Exporter>();

                // SquarifiedTreemapForge.WinForms.csproj
                services.AddTransient<TreemapGdiDriver<PivotDataSource>>();
                services.AddTransient<IGdiRenderer, GdiRenderer>();

                // SquarifiedTreemapForge.csproj
                services.AddTransient<LayoutInteractor<PivotDataSource>>();
                services.AddTransient<LayoutGenerator<PivotDataSource>>();
                services.AddTransient<DataGroupPreparer<PivotDataSource>>();
                services.AddTransient<LegendGenerator>();
                services.AddTransient<ITreemapGenerator, SquarifiedTreemapGenerator>();
            });
}
