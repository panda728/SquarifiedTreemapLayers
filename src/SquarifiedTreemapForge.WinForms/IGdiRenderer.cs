using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapForge.WinForms;

public interface IGdiRenderer
{
    Action<Graphics, TreemapNode>? DrawLeafNode { get; set; }
    int GetFontHeight(Graphics g, NodeFont font, string text);
    void Render(
        Graphics g,
        Treemap? treemap,
        TreemapNode rootNode,
        int displayMinSize = 20,
        IEnumerable<Legend>? legends = null,
        Rectangle? highLightBounds = null);
}
