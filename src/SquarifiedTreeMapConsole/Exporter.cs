using System.Drawing;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SquarifiedTreeMapCoordinator;
using SquarifiedTreeMapForge.WinForms;
using SquarifiedTreeMapShared;

namespace SquarifiedTreeMapConsole;

public sealed class Exporter(
    GdiRenderer renderer,
    LayoutCoordinator<PivotDataSource> coordinator,
    IOptions<TreeMapSettings> treeMapSettingsOp,
    IOptions<TreeMapLayoutSettings> layoutSettingsOp,
    IOptions<LegendSettings> legendSettingsOp)
{
    public TreeMapSettings TreeMapSettings { get; set; } = treeMapSettingsOp.Value;
    public TreeMapLayoutSettings LayoutSettings { get; set; } = layoutSettingsOp.Value;
    public LegendSettings LegendSettings { get; set; } = legendSettingsOp.Value;

    public string Export(int width, int height, string dataPath, string? pngPath)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        if (!File.Exists(dataPath)) { throw new FileNotFoundException("Data file not found.", dataPath); }

        var json = File.ReadAllText(dataPath);
        var data = JsonSerializer.Deserialize<IEnumerable<PivotDataSource>>(json) ?? [];

        coordinator.SetDataSource(
            data, LayoutSettings, TreeMapSettings, LegendSettings, GetTitle, GetColor);
        
        var bmp = Render(width, height) ?? throw new ApplicationException("no data.");

        if (pngPath != null)
        {
            bmp.Save(pngPath, System.Drawing.Imaging.ImageFormat.Png);
            return new FileInfo(pngPath).FullName;
        }

        var ms = new MemoryStream();
        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
    }

    string GetTitle(string text, IEnumerable<PivotDataSource> d)
    {
        var total = (double)d.Sum(d => d.Weight);
        var purchaseTotal = (double)d.Sum(d => d.RelativeWeight);
        var per = purchaseTotal / total;
        return $"{text}({d.Sum(d => d.Weight) / 1000:#,##0} {per:+0.0%;-0.0%}) ";
    }

    Color GetColor(IEnumerable<PivotDataSource> d)
    {
        var total = (double)d.Sum(d => d.Weight);
        var diff = (double)d.Sum(d => d.RelativeWeight);
        var percentage = Math.Round(diff / total * 1000) / 1000;
        return coordinator.GetPercentageColor(percentage);
    }

    /// <summary>Renders the treemap to a bitmap.</summary>
    public Bitmap? Render(int width, int height)
    {
        var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);

        var nodeHeight = renderer.GetFontHeight(
            g, coordinator.NodeFont, LayoutSettings.TitleText);

        if (coordinator.Layout(new Rectangle(Point.Empty, bmp.Size), nodeHeight)
            && coordinator.TreeMap != null && coordinator.RootNode != null)
        {
            renderer.Render(g, coordinator.TreeMap, coordinator.RootNode, nodeHeight);
            return bmp;
        }
        throw new ApplicationException("no data.");
    }
}
