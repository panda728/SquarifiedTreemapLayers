using System.Drawing;
using SquarifiedTreemapForge.Helpers;
using SquarifiedTreemapForge.Layout;
using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapForge;

public class LayoutInteractor<T>(
    LayoutGenerator<T> layoutGenerator,
    DataGroupPreparer<T> preparer,
    LegendGenerator legend)
{
    bool _isSourceOrderDec = false;
    LayoutAlign _layoutAlign = LayoutAlign.LeftTop;

    Seed<T>? _rootSeed;

    public Treemap? Treemap { get; set; }
    public NodeFont TitleNodeFont { get; set; } = new();
    public NodeFont NodeFont { get; set; } = new();
    public NodeFont LegendFont { get; set; } = new();
    public TreemapNode? RootNode { get; set; }
    public HashSet<int> Filter { get; set; } = [];
    public Rectangle? HighLightBounds { get; set; }

    public void SetHighlight(Rectangle bounds)
    {
        if (HighLightBounds != null && HighLightBounds.Equals(bounds)) return;
        HighLightBounds = bounds;
    }

    public bool ResetHighlight()
    {
        if (HighLightBounds == null) { return false; }
        HighLightBounds = null;
        return true;
    }

    /// <summary>Sets the data source for the treemap.</summary>
    public void SetDataSource(
        IEnumerable<T> sources,
        TreemapLayoutSettings settings,
        TreemapSettings treemapSettings,
        LegendSettings legendSettings,
        Func<string, IEnumerable<T>, string>? funcNodeText = null,
        Func<IEnumerable<T>, Color>? funcNodeColor = null,
        Func<IEnumerable<T>, double>? funcPercentage = null)
    {
        _isSourceOrderDec = settings.IsSourceOrderDec;
        _layoutAlign = settings.LayoutAlign;

        legend.LoadSettings(legendSettings);
        FuncPercentage = funcPercentage;

        preparer.Initialize(settings, funcNodeText, funcNodeColor ?? GetColor);

        _rootSeed = DataGrouper<T>.CreateSeed(
            sources,
            settings,
            treemapSettings,
            preparer
        );

        TitleNodeFont = new NodeFont(
            treemapSettings.TitleFontFamily,
            treemapSettings.TitleFontSize,
            treemapSettings.IsTitleFontBold
        );

        NodeFont = new NodeFont(
            treemapSettings.NodeFontFamily,
            treemapSettings.NodeFontSize,
            treemapSettings.IsNodeFontBold
        );

        LegendFont = new NodeFont(
            treemapSettings.LegendFontFamily,
            treemapSettings.LegendFontSize,
            treemapSettings.IsLegendFontBold
        );

        var format = new NodeFormat(
            treemapSettings.ForeColor,
            treemapSettings.BackColor,
            1,
            Color.White
        );

        Treemap = new Treemap(
            settings.TitleText,
            TitleNodeFont,
            format,
            treemapSettings.Margin,
            NodeFont,
            LegendFont,
            treemapSettings.HighlightColor
        );
    }

    public Func<IEnumerable<T>, double>? FuncPercentage { get; set; }

    Color GetColor(IEnumerable<T> d) => FuncPercentage == null
        ? ColorHelper.GenerateRandomColor()
        : legend.GetPercentageColor(FuncPercentage.Invoke(d));

    /// <summary>Calculates the layout of the treemap.</summary>
    public bool Layout(Rectangle bounds, int nodeFontHeight)
    {
        if (nodeFontHeight <= 0) { nodeFontHeight = 1; }

        if (_rootSeed == null || bounds.Width < nodeFontHeight || bounds.Height < nodeFontHeight)
        {
            RootNode = null;
            return false;
        }
        RootNode = layoutGenerator.Layout(
            _rootSeed, _isSourceOrderDec, _layoutAlign, null, _rootSeed.Children, bounds, nodeFontHeight, Filter);
        return RootNode != null;
    }

    /// <summary>Clears the current layout and data.</summary>
    public void Clear()
    {
        _rootSeed = null;
        Treemap = null;
        RootNode = null;
    }

    public TreemapNode? GetContainsItem(Point p) => RootNode?.GetContainsItem(p);

    public bool SetFilterIfContains(Point p)
    {
        HighLightBounds = null;
        var node = GetContainsItem(p);
        if (node != null)
        {
            Filter = node.GetFilter();
            return true;
        }

        if (Filter.Count == 0) { return false; }
        Filter.Clear();
        return false;
    }

    public bool SetHighLightIfContains(Point p)
    {
        var node = GetContainsItem(p);
        if (node == null) { return ResetHighlight(); }
        SetHighlight(node.Bounds);
        return true;
    }

    public Legend[] GenerateLegends(Rectangle bounds)
        => legend.GenerateLegends(bounds);

    public Color GetPercentageColor(double per) => legend.GetPercentageColor(per);
}
