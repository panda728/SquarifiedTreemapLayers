using System.Drawing;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SquarifiedTreemapInteractor;
using SquarifiedTreemapForge.WinForms;
using SquarifiedTreemapShared;

namespace SquarifiedTreemapConsole;

public sealed class Exporter(
    GdiRenderer renderer,
    LayoutInteractor<PivotDataSource> interactor,
    IOptions<TreemapSettings> treemapSettingsOp,
    IOptions<TreemapLayoutSettings> layoutSettingsOp,
    IOptions<LegendSettings> legendSettingsOp)
{
    public TreemapSettings TreemapSettings { get; set; } = treemapSettingsOp.Value;
    public TreemapLayoutSettings LayoutSettings { get; set; } = layoutSettingsOp.Value;
    public LegendSettings LegendSettings { get; set; } = legendSettingsOp.Value;

    public string Export(int width, int height, string dataPath, string? pngPath)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        if (!File.Exists(dataPath)) { throw new FileNotFoundException("Data file not found.", dataPath); }

        var json = File.ReadAllText(dataPath);
        var data = JsonSerializer.Deserialize<IEnumerable<PivotDataSource>>(json) ?? [];

        interactor.SetDataSource(
            data, LayoutSettings, TreemapSettings, LegendSettings, GetTitle, GetColor);

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

    static string GetTitle(string text, IEnumerable<PivotDataSource> d)
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
        return interactor.GetPercentageColor(percentage);
    }

    /// <summary>Renders the treemap to a bitmap.</summary>
    public Bitmap? Render(int width, int height)
    {
        var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);

        var nodeHeight = renderer.GetFontHeight(
            g, interactor.NodeFont, LayoutSettings.TitleText);

        if (interactor.Layout(new Rectangle(Point.Empty, bmp.Size), nodeHeight)
            && interactor.Treemap != null && interactor.RootNode != null)
        {
            renderer.Render(g, interactor.Treemap, interactor.RootNode, nodeHeight);
            return bmp;
        }
        throw new ApplicationException("no data.");
    }
}
