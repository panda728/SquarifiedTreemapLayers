using System.ComponentModel;
using Microsoft.Extensions.Options;
using SquarifiedTreeMapCoordinator;
using SquarifiedTreeMapShared;

namespace SquarifiedTreeMapForge.WinForms;

/// <summary>Manages the data source and rendering for a treemap.</summary>
public sealed class TreeMapGdiDriver<T>(
    IOptions<TreeMapSettings> treeMapSettingsOp,
    IOptions<TreeMapLayoutSettings> layoutSettingsOp,
    IOptions<LegendSettings> legendSettingsOp,
    LayoutCoordinator<T> coordinator,
    GdiRenderer renderer
)
{
    readonly Semaphore _semaphore = new(1, 1);

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreeMapSettings TreeMapSettings { get; set; } = treeMapSettingsOp.Value;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreeMapLayoutSettings LayoutSettings { get; set; } = layoutSettingsOp.Value;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LegendSettings LegendSettings { get; set; } = legendSettingsOp.Value;

    public Func<string, IEnumerable<T>, string>? FuncNodeText { get; set; }
    public Func<IEnumerable<T>, Color>? FuncNodeColor { get; set; }

    TreeMapControl? _treeMapControl;
    public TreeMapControl? TreeMapControl
    {
        get { return _treeMapControl; }
        set
        {
            if (_treeMapControl != null)
            {
                _treeMapControl.OnPaintAction = null;
                _treeMapControl.OnDoubleClickAction -= treeMapControl_DoubleClick;
                _treeMapControl.OnMouseMoveAction -= treeMapControl_MoveAction;
                _treeMapControl.OnMouseLeaveAction -= treeMapControl_LeaveAction;
            }

            _treeMapControl = value;

            if (_treeMapControl != null)
            {
                _treeMapControl.OnPaintAction = OnPaint;
                _treeMapControl.OnDoubleClickAction += treeMapControl_DoubleClick;
                _treeMapControl.OnMouseMoveAction += treeMapControl_MoveAction;
                _treeMapControl.OnMouseLeaveAction += treeMapControl_LeaveAction;
            }
        }
    }

    public Color GetPercentageColor(double per)
        => coordinator.GetPercentageColor(per);

    public void Invalidate() => _treeMapControl?.Invalidate();

    public void Invalidate(IEnumerable<T> sources)
    {
        coordinator.SetDataSource(
            sources, LayoutSettings, TreeMapSettings, LegendSettings, FuncNodeText, FuncNodeColor);
        _treeMapControl?.Invalidate();
    }

    public void OnPaint(PaintEventArgs e)
    {
        if (_semaphore.WaitOne(0) == false) return;
        try
        {
            RenderTreeMap(e.Graphics, coordinator, renderer, e.ClipRectangle, LayoutSettings.TitleText);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    static void RenderTreeMap(
        Graphics g,
        LayoutCoordinator<T> coordinator,
        GdiRenderer renderer,
        Rectangle bounds,
        string titleText)
    {
        var nodeHeight = renderer.GetFontHeight(
            g, coordinator.NodeFont, titleText);

        if (coordinator.Layout(bounds, nodeHeight)
            && coordinator.TreeMap != null && coordinator.RootNode != null)
        {
            var legends = coordinator.GenerateLegends(bounds);
            renderer.Render(
                g, coordinator.TreeMap, coordinator.RootNode, nodeHeight, legends, coordinator.HighLightBounds);
        }
    }

    #region Mouse Event
    public Action<object?, EventArgs>? OnDoubleClickAction { get; set; }
    public Action<object?, MouseEventArgs>? OnMouseMoveAction { get; set; }
    public Action<object?, EventArgs>? OnMouseLeaveAction { get; set; }

    public TreeMapNode? GetContainsItem(Point cp)
        => coordinator.GetContainsItem(cp);

    void treeMapControl_DoubleClick(object? sender, EventArgs e)
    {
        if (_treeMapControl == null)
        {
            OnDoubleClickAction?.Invoke(this, e);
            return;
        }

        var cp = _treeMapControl.PointToClient(Cursor.Position);
        coordinator.SetFilterIfContains(cp);
        _treeMapControl?.Invalidate();
    }

    void treeMapControl_MoveAction(object? sender, MouseEventArgs e)
    {
        if (_treeMapControl == null)
        {
            OnMouseMoveAction?.Invoke(this, e);
            return;
        }

        var cp = _treeMapControl.PointToClient(Cursor.Position);
        if (coordinator.SetHighLightIfContains(cp))
        {
            _treeMapControl?.Invalidate();
        }
        OnMouseMoveAction?.Invoke(this, e);
    }

    void treeMapControl_LeaveAction(object? sender, EventArgs e)
    {
        if (_treeMapControl != null && coordinator.ResetHighlight())
        {
            _treeMapControl?.Invalidate();
        }
        OnMouseLeaveAction?.Invoke(this, e);
    }
    #endregion

    /// <summary>Renders the treemap to a bitmap.</summary>
    public Bitmap Render(int width, int height)
    {
        var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);
        RenderTreeMap(g, coordinator, renderer, new Rectangle(Point.Empty, bmp.Size), LayoutSettings.TitleText);
        return bmp;
    }
}
