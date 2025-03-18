using System.Drawing;
using SquarifiedTreeMapForge.Layout;
using SquarifiedTreeMapShared;

namespace SquarifiedTreeMapInteractor;

public class LayoutInteractor<T>(
    LayoutGenerator<T> layoutGenerator,
    DataGroupPreparer<T> preparer,
    LegendCalculator legend)
{
    bool _isSourceOrderDec = false;
    LayoutAlign _layoutAlign = LayoutAlign.LeftTop;

    Seed<T>? _rootSeed;

    public TreeMap? TreeMap { get; set; }
    public NodeFont TitleNodeFont { get; set; } = new();
    public NodeFont NodeFont { get; set; } = new();
    public NodeFont LegendFont { get; set; } = new();
    public TreeMapNode? RootNode { get; set; }
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
        TreeMapLayoutSettings settings,
        TreeMapSettings treeMapSettings,
        LegendSettings legendSettings,
        Func<string, IEnumerable<T>, string>? funcNodeText,
        Func<IEnumerable<T>, Color>? funcNodeColor)
    {
        _isSourceOrderDec = settings.IsSourceOrderDec;
        _layoutAlign = settings.LayoutAlign;

        preparer.Initialize(settings, funcNodeText, funcNodeColor);
        legend.LoadSettings(legendSettings);

        _rootSeed = DataGrouper<T>.CreateSeed(
            sources,
            settings,
            treeMapSettings,
            preparer
        );

        TitleNodeFont = new NodeFont(
            treeMapSettings.TitleFontFamily,
            treeMapSettings.TitleFontSize,
            treeMapSettings.IsTitleFontBold
        );

        NodeFont = new NodeFont(
            treeMapSettings.NodeFontFamily,
            treeMapSettings.NodeFontSize,
            treeMapSettings.IsNodeFontBold
        );

        LegendFont = new NodeFont(
            treeMapSettings.LegendFontFamily,
            treeMapSettings.LegendFontSize,
            treeMapSettings.IsLegendFontBold
        );

        var format = new NodeFormat(
            treeMapSettings.ForeColor,
            treeMapSettings.BackColor,
            1,
            Color.White
        );

        TreeMap = new TreeMap(
            settings.TitleText,
            TitleNodeFont,
            format,
            treeMapSettings.Margin,
            NodeFont,
            LegendFont,
            treeMapSettings.HighlightColor
        );
    }

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
        TreeMap = null;
        RootNode = null;
    }

    public TreeMapNode? GetContainsItem(Point p) => RootNode?.GetContainsItem(p);

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
