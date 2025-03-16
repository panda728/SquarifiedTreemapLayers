using System.ComponentModel;

namespace SquarifiedTreeMapForge.WinForms;

public partial class TreeMapControl : Control
{
    public TreeMapControl()
    {
        InitializeComponent();
        DoubleBuffered = true;
    }

    private void TreeMapControl_SizeChanged(object sender, EventArgs e)
    {
        this.Invalidate();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Action<PaintEventArgs>? OnPaintAction { get; set; }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        OnPaintAction?.Invoke(e);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Action<object?, EventArgs>? OnDoubleClickAction { get; set; }

    protected override void OnDoubleClick(EventArgs e)
    {
        base.OnDoubleClick(e);
        OnDoubleClickAction?.Invoke(this, e);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Action<object?, MouseEventArgs>? OnMouseMoveAction { get; set; }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        OnMouseMoveAction?.Invoke(this, e);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Action<object?, EventArgs>? OnMouseLeaveAction { get; set; }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        OnMouseLeaveAction?.Invoke(this, e);
    }
}
