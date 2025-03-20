using SquarifiedTreemapShared;

namespace SquarifiedTreemapForge.WinForms;

public interface IGdiRenderer
{
    int GetFontHeight(Graphics g, NodeFont font, string text);
    void Render(
        Graphics g,
        Treemap? treemap,
        TreemapNode rootNode,
        int displayMinSize = 20,
        IEnumerable<Legend>? legends = null,
        Rectangle? highLightBounds = null);
}
