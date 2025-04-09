using System.ComponentModel;
using Microsoft.Extensions.Options;
using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapForge.WinForms;

/// <summary>Manages the data source and rendering for a treemap.</summary>
public sealed class TreemapGdiDriver<T>(
    IGdiRenderer renderer,
    LayoutInteractor<T> interactor,
    IOptions<TreemapSettings> treemapSettingsOp,
    IOptions<TreemapLayoutSettings> layoutSettingsOp,
    IOptions<LegendSettings> legendSettingsOp
    )
{
    const string FONT_HEIGHT_DEFALUT = "A";

    readonly Semaphore _semaphore = new(1, 1);
    TreemapControl? _treemapControl;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreemapSettings TreemapSettings { get; set; } = treemapSettingsOp.Value;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreemapLayoutSettings LayoutSettings { get; set; } = layoutSettingsOp.Value;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LegendSettings LegendSettings { get; set; } = legendSettingsOp.Value;

    public Func<string, IEnumerable<T>, string>? FuncNodeText { get; set; }
    public Func<IEnumerable<T>, Color>? FuncNodeColor { get; set; }
    public Func<IEnumerable<T>, double>? FuncPercentage { get; set; }
    public Action<Graphics, TreemapNode, IEnumerable<T>>? DrawLeafNode { get; set; }

    public Treemap? Treemap { get => interactor.Treemap; }
    public TreemapNode? RootNode { get => interactor.RootNode; }

    public TreemapControl? TreemapControl
    {
        get { return _treemapControl; }
        set
        {
            if (_treemapControl != null)
            {
                _treemapControl.OnPaintAction = null;
                _treemapControl.OnDoubleClickAction -= treemapControl_DoubleClick;
                _treemapControl.OnMouseMoveAction -= treemapControl_MoveAction;
                _treemapControl.OnMouseLeaveAction -= treemapControl_LeaveAction;
            }

            _treemapControl = value;

            if (_treemapControl != null)
            {
                _treemapControl.OnPaintAction = OnPaint;
                _treemapControl.OnDoubleClickAction += treemapControl_DoubleClick;
                _treemapControl.OnMouseMoveAction += treemapControl_MoveAction;
                _treemapControl.OnMouseLeaveAction += treemapControl_LeaveAction;
            }
        }
    }

    public Color GetPercentageColor(double per)
        => interactor.GetPercentageColor(per);

    public void ResetFilter() => interactor.ResetFilter();

    public void Invalidate() => _treemapControl?.Invalidate();

    public void Invalidate(IEnumerable<T> sources, bool withFilterRest = true)
    {
        if (withFilterRest) { ResetFilter(); }
        renderer.DrawLeafNode ??= DrawLeafNodeInner;
        interactor.SetDataSource(
            sources,
            LayoutSettings,
            TreemapSettings,
            LegendSettings,
            FuncNodeText,
            FuncNodeColor,
            FuncPercentage);
        _treemapControl?.Invalidate();
    }

    public void OnPaint(PaintEventArgs e)
    {
        if (_semaphore.WaitOne(0) == false) return;
        try
        {
            RenderTreemap(e.Graphics, interactor, renderer, e.ClipRectangle,
                LayoutSettings.TitleText, LayoutSettings.DisplayMaxDepth, LayoutSettings.NodeTextHeightMargin);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    static void RenderTreemap(
        Graphics g,
        LayoutInteractor<T> interactor,
        IGdiRenderer renderer,
        Rectangle bounds,
        string titleText,
        int displayMaxDepth,
        int nodeTextHeightMargin = 0)
    {
        if (bounds.Width <= 0 || bounds.Height <= 0) { return; }

        var nodeHeight = renderer.GetFontHeight(
            g, interactor.NodeFont, string.IsNullOrEmpty(titleText) ? FONT_HEIGHT_DEFALUT : titleText);

        if (nodeTextHeightMargin > 0)
        {
            nodeHeight += nodeTextHeightMargin;
        }

        if (interactor.Layout(bounds, nodeHeight, displayMaxDepth)
            && interactor.Treemap != null && interactor.RootNode != null)
        {
            var legends = interactor.GenerateLegends(bounds);
            renderer.Render(
                g, interactor.Treemap, interactor.RootNode, nodeHeight, legends, interactor.HighLightBounds);
        }
    }

    void DrawLeafNodeInner(Graphics g, TreemapNode node)
    {
        if (DrawLeafNode == null) { return; }
        DrawLeafNode.Invoke(g, node, interactor.GetSources(node));
    }

    #region Mouse Event
    public Action<object?, EventArgs>? OnDoubleClickAction { get; set; }
    public Action<object?, MouseEventArgs>? OnMouseMoveAction { get; set; }
    public Action<object?, EventArgs>? OnMouseLeaveAction { get; set; }

    public TreemapNode? GetContainsItem(Point cp)
        => interactor.GetContainsItem(cp);
    public IEnumerable<T> GetSources(Point cp)
        => interactor.GetSources(cp);

    void treemapControl_DoubleClick(object? sender, EventArgs e)
    {
        if (_treemapControl == null)
        {
            OnDoubleClickAction?.Invoke(this, e);
            return;
        }

        var cp = _treemapControl.PointToClient(Cursor.Position);
        interactor.SetFilterIfContains(cp);
        _treemapControl?.Invalidate();
    }

    void treemapControl_MoveAction(object? sender, MouseEventArgs e)
    {
        if (_treemapControl == null)
        {
            OnMouseMoveAction?.Invoke(this, e);
            return;
        }

        var cp = _treemapControl.PointToClient(Cursor.Position);
        if (interactor.SetHighLightIfContains(cp))
        {
            _treemapControl?.Invalidate();
        }
        OnMouseMoveAction?.Invoke(this, e);
    }

    void treemapControl_LeaveAction(object? sender, EventArgs e)
    {
        if (_treemapControl != null && interactor.ResetHighlight())
        {
            _treemapControl?.Invalidate();
        }
        OnMouseLeaveAction?.Invoke(this, e);
    }
    #endregion

    /// <summary>Renders the treemap to a bitmap.</summary>
    public Bitmap Render(int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width and height must be positive values.");

        var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);
        RenderTreemap(g, interactor, renderer, new Rectangle(Point.Empty, bmp.Size),
            LayoutSettings.TitleText, LayoutSettings.DisplayMaxDepth, LayoutSettings.NodeTextHeightMargin);
        return bmp;
    }
}
