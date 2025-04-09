using System.Drawing.Drawing2D;
using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapForge.WinForms;

public class GdiRenderer : IGdiRenderer, IDisposable
{
    readonly Dictionary<Color, SolidBrush> _brushCache = [];
    readonly Dictionary<NodeFont, Font> _fontCache = [];
    readonly Dictionary<(Color, float), Pen> _penCache = [];

    bool _disposed = false;

    public Action<Graphics, TreemapNode>? DrawLeafNode { get; set; }
    public int NodeDepthLimit { get; set; } = 255;
    public int NodeSizeLimit { get; set; } = int.MaxValue / 2;

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
        Treemap? treemap,
        TreemapNode rootNode,
        int displayMinSize = 20,
        IEnumerable<Legend>? legends = null,
        Rectangle? highLightBounds = null)
    {
        if (treemap == null)
        {
            g.Clear(Color.Black);
            return;
        }

        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(treemap.Format.BackColor);
        if (!string.IsNullOrEmpty(treemap.Text))
        {
            RenderTitle(g, treemap);
        }

        if (legends != null && legends.Any() && displayMinSize > 0)
        {
            RenderLegends(g, treemap, legends);
        }

        RenderTreemapContent(g, treemap, rootNode, displayMinSize);

        if (highLightBounds != null)
        {
            var pen = GetPen(treemap.HighlightColor, treemap.Format.BorderWidth);
            g.DrawRectangle(pen, highLightBounds.Value);
        }
        CacheClear();
    }

    void RenderTreemapContent(
        Graphics g, Treemap treemap, TreemapNode rootNode, int displayMinSize)
    {
        var nodeFont = GetFont(treemap.NodeFont);
        foreach (var child in rootNode.Nodes)
        {
            RenderNodes(g, child, nodeFont, displayMinSize);
        }
    }

    void RenderTitle(Graphics g, Treemap treemap)
    {
        var textBrush = GetBrush(treemap.Format.ForeColor);
        var titleFont = GetFont(treemap.TextFont);
        g.DrawString(treemap.Text, titleFont, textBrush, 0, 0);
    }

    void RenderLegends(Graphics g, Treemap treemap, IEnumerable<Legend> legends)
    {
        var textBrush = GetBrush(treemap.Format.ForeColor);
        var legendFont = GetFont(treemap.LegendFont);
        foreach (var legend in legends)
        {
            var brush = GetBrush(legend.BackColor);
            g.FillRectangle(brush, legend.Bounds);

            var textSize = g.MeasureString(legend.Text, legendFont);
            var textX = legend.TextBounds.X + (legend.TextBounds.Width - textSize.Width) / 2;
            var textY = legend.TextBounds.Y + (legend.TextBounds.Height - textSize.Height) / 2;
            var textPosition = new PointF(textX, textY);

            g.DrawString(legend.Text, legendFont, textBrush, textPosition);
        }
    }

    void RenderNodes(Graphics g, TreemapNode rootNode, Font nodeFont, int displayMinSize)
    {
        var stack = new Stack<(TreemapNode node, int depth)>();
        stack.Push((rootNode, 0));

        var visitedNodes = new HashSet<int>();

        while (stack.Count > 0)
        {
            var (node, depth) = stack.Pop();

            if (depth > NodeDepthLimit || visitedNodes.Contains(node.Id))
            {
                continue;
            }

            visitedNodes.Add(node.Id);

            RenderNode(g, node, nodeFont, displayMinSize);

            foreach (var child in node.Nodes)
            {
                stack.Push((child, depth + 1));
            }
        }
    }

    public void RenderNode(Graphics g, TreemapNode node, Font nodeFont, int displayMinSize)
    {
        if (!IsNodeDrawable(node, displayMinSize)) { return; }

        DrawNodeBackground(g, node);

        if (ShouldDrawText(node, displayMinSize))
        {
            DrawNodeText(g, node, nodeFont);
            InvokeDrawLeafNode(g, node);
        }

        DrawNodeBorder(g, node);
    }

    bool IsNodeDrawable(TreemapNode node, int displayMinSize)
        => node.Bounds.Width > displayMinSize && node.Bounds.Height > displayMinSize &&
            node.Bounds.Width <= NodeSizeLimit && node.Bounds.Height <= NodeSizeLimit;

    void DrawNodeBackground(Graphics g, TreemapNode node)
    {
        var brush = GetBrush(node.Format.BackColor);
        g.FillRectangle(brush, node.Bounds);
    }

    static bool ShouldDrawText(TreemapNode node, int displayMinSize)
        => node.Bounds.Width > displayMinSize &&
               node.Bounds.Height > displayMinSize &&
               !string.IsNullOrEmpty(node.Text);

    void DrawNodeText(Graphics g, TreemapNode node, Font nodeFont)
    {
        var textArea = node.Bounds;

        var textMergin = (int)(node.Format.BorderWidth > 0
            ? -2 : -2 + Math.Ceiling(node.Format.BorderWidth / 2.0));
        textArea.Inflate(textMergin, textMergin);

        if (textArea.Width <= 0 || node.Bounds.Height < node.FontHeight)
        {
            return;
        }

        if (node.Nodes.Count > 0 || textArea.Height < node.FontHeight * 2)
        {
            textArea.Height = node.FontHeight;
        }

        var textBrush = GetBrush(node.Format.ForeColor);
        g.DrawString(node.Text, nodeFont, textBrush, textArea);
    }

    private void InvokeDrawLeafNode(Graphics g, TreemapNode node)
    {
        if (node.Nodes.Count == 0)
        {
            DrawLeafNode?.Invoke(g, node);
        }
    }

    void DrawNodeBorder(Graphics g, TreemapNode node)
    {
        var pen = GetPen(node.Format.BorderColor, node.Format.BorderWidth);
        g.DrawRectangle(pen, node.Bounds);
    }
}
