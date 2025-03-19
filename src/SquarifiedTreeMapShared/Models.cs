using System.Drawing;

namespace SquarifiedTreemapShared;

public enum LayoutAlign
{
    LeftTop = 0,
    RightTop = 1,
    LeftBottom = 2,
    RightBottom = 3,
    Alternating = 4,
}

public record NodeFont(
    string FamilyName = "Arial",
    float Size = 9.5F,
    bool IsBold = false
);

public record NodeFormat(
    Color ForeColor,
    Color BackColor,
    int BorderWidth,
    Color BorderColor
);

public record Treemap(
    string Text,
    NodeFont TextFont,
    NodeFormat Format,
    int Margin,
    NodeFont NodeFont,
    NodeFont LegendFont,
    Color HighlightColor);

public record TreemapNode(
    int Id,
    int Depth,
    string Text,
    int FontHeight,
    Rectangle Bounds,
    NodeFormat Format,
    TreemapNode? Parent,
    List<TreemapNode> Nodes)
{
    public TreemapNode? GetContainsItem(Point p) => GetContains(p, Nodes);

    static TreemapNode? GetContains(Point p, IEnumerable<TreemapNode> nodes)
    {
        if (nodes == null || !nodes.Any()) return null;
        foreach (var n in nodes)
        {
            if (n.Bounds.Contains(p))
            {
                return GetContains(p, n.Nodes) ?? n;
            }
        }
        return null;
    }

    public IReadOnlyList<string> GetAllPathTexts()
    {
        if (Parent is null) { return []; }
        List<string> titles = [Text];
        var p = this;
        while (true)
        {
            var text = p.Parent?.Text ?? "";
            titles.Add(string.IsNullOrWhiteSpace(text) ? "-" : text);
            if (p.Parent is null || p.Depth < 0) { break; }
            p = p.Parent;
        }
        titles.Reverse();
        return titles;
    }

    public HashSet<int> GetFilter()
    {
        if (Parent is null) { return []; }
        HashSet<int> filter = [];
        var p = this;
        while (true)
        {
            filter.Add(p.Id);
            if (p.Parent is null) { break; }
            p = p.Parent;
        }
        return filter;
    }
};

public record Legend(
    Rectangle Bounds,
    Color BackColor,
    string Text,
    Rectangle TextBounds
);

public record PivotDataSource(
  double Weight,
  double RelativeWeight,
  string Group1,
  string Group2 = "",
  string Group3 = "",
  string Group4 = "",
  string Group5 = "",
  string Group6 = "",
  string Group7 = "",
  string Group8 = "",
  string Group9 = ""
);
