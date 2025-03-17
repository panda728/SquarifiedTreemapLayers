using System.Drawing.Drawing2D;
using SquarifiedTreeMapShared;

namespace SquarifiedTreeMapForge.WinForms;

public class GdiRenderer : IDisposable
{
    bool _disposed = false;
    readonly Dictionary<Color, SolidBrush> _brushCache = [];
    readonly Dictionary<NodeFont, Font> _fontCache = [];
    readonly Dictionary<(Color, float), Pen> _penCache = [];

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) { CacheClear(); }
        _disposed = true;
    }

    public void CacheClear()
    {
        foreach (var brush in _brushCache.Values) { brush.Dispose(); }
        _brushCache.Clear();
        foreach (var font in _fontCache.Values) { font.Dispose(); }
        _fontCache.Clear();
        foreach (var pen in _penCache.Values) { pen.Dispose(); }
        _penCache.Clear();
    }

    SolidBrush GetBrush(Color color)
    {
        if (!_brushCache.TryGetValue(color, out var brush))
        {
            brush = new SolidBrush(color);
            _brushCache[color] = brush;
        }
        return brush;
    }

    Font GetFont(NodeFont fontSetting)
    {
        if (!_fontCache.TryGetValue(fontSetting, out var font))
        {
            var style = fontSetting.IsBold ? FontStyle.Bold : FontStyle.Regular;
            font = new Font(fontSetting.FamilyName, fontSetting.Size, style);
            _fontCache[fontSetting] = font;
        }
        return font;
    }

    Pen GetPen(Color color, float width)
    {
        var key = (color, width);
        if (!_penCache.TryGetValue(key, out var pen))
        {
            pen = new Pen(color, width);
            _penCache[key] = pen;
        }
        return pen;
    }

    public int GetFontHeight(Graphics g, NodeFont font, string text)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var f = GetFont(font);
        var measureTitle = g.MeasureString(text, f);
        return (int)measureTitle.Height;
    }

    public void Render(
        Graphics g,
        TreeMap? treeMap,
        TreeMapNode rootNode,
        int displayMinSize = 20,
        IEnumerable<Legend>? legends = null,
        Rectangle? highLightBounds = null)
    {
        if (treeMap == null)
        {
            g.Clear(Color.Black);
            return;
        }

        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(treeMap.Format.BackColor);
        if (!string.IsNullOrEmpty(treeMap.Text))
        {
            RenderTitle(g, treeMap);
        }

        if (legends != null && legends.Any() && displayMinSize > 0)
        {
            RenderLegends(g, treeMap, legends);
        }

        RenderTreeMapContent(g, treeMap, rootNode, displayMinSize);

        if (highLightBounds == null) { return; }
        var pen = GetPen(treeMap.HighlightColor, treeMap.Format.BorderWidth);
        g.DrawRectangle(pen, highLightBounds.Value);

        CacheClear();
    }

    void RenderTreeMapContent(
        Graphics g, TreeMap treeMap, TreeMapNode rootNode, int displayMinSize)
    {
        var nodeFont = GetFont(treeMap.NodeFont);
        foreach (var child in rootNode.Nodes)
        {
            RenderNodes(g, child, nodeFont, displayMinSize);
        }
    }

    void RenderTitle(Graphics g, TreeMap treeMap)
    {
        var brushText = GetBrush(treeMap.Format.ForeColor);
        var titleFont = GetFont(treeMap.TextFont);
        g.DrawString(treeMap.Text, titleFont, brushText, 0, 0);
    }

    void RenderLegends(Graphics g, TreeMap treeMap, IEnumerable<Legend> legends)
    {
        var brushText = GetBrush(treeMap.Format.ForeColor);
        var legendFont = GetFont(treeMap.LegendFont);
        foreach (var legend in legends)
        {
            var brush = GetBrush(legend.BackColor);
            g.FillRectangle(brush, legend.Bounds);

            var textSize = g.MeasureString(legend.Text, legendFont);
            var textX = legend.TextBounds.X + (legend.TextBounds.Width - textSize.Width) / 2;
            var textY = legend.TextBounds.Y + (legend.TextBounds.Height - textSize.Height) / 2;
            var textPosition = new PointF(textX, textY);

            g.DrawString(legend.Text, legendFont, brushText, textPosition);
        }
    }

    void RenderNodes(Graphics g, TreeMapNode node, Font nodeFont, int displayMinSize)
    {
        RenderNode(g, node, nodeFont, displayMinSize);
        foreach (var child in node.Nodes)
        {
            RenderNodes(g, child, nodeFont, displayMinSize);
        }
    }

    public void RenderNode(Graphics g, TreeMapNode node, Font nodeFont, int displayMinSize)
    {
        var brush = GetBrush(node.Format.BackColor);
        g.FillRectangle(brush, node.Bounds);

        if (node.Bounds.Width > displayMinSize
            && node.Bounds.Height > displayMinSize
            && !string.IsNullOrEmpty(node.Text))
        {
            var textArea = node.Bounds;
            textArea.Inflate(-2, -2);
            if (node.Bounds.Height >= node.FontHeight)
            {
                if (node.Nodes.Count > 0 || textArea.Height < node.FontHeight * 2)
                {
                    textArea.Height = node.FontHeight;
                }
                var brushText = GetBrush(node.Format.ForeColor);
                g.DrawString(node.Text, nodeFont, brushText, textArea);
            }
        }
        var pen = GetPen(node.Format.BorderColor, node.Format.BorderWidth);
        g.DrawRectangle(pen, node.Bounds);
    }
}
