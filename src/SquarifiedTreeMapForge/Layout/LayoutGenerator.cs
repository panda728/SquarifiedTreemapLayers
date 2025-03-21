using System.Drawing;
using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapForge.Layout;

/// <summary>Manages the layout of a treemap.</summary>
public sealed class LayoutGenerator<T>(ITreemapGenerator treemapGenerator)
{
    /// <summary>Recursively calculates the layout of the treemap nodes.</summary>
    public TreemapNode Layout(
        Seed<T> current,
        bool isSourceOrderDec,
        LayoutAlign layoutAlign,
        TreemapNode? parent,
        IEnumerable<Seed<T>> children,
        Rectangle bounds,
        int height,
        HashSet<int> filter)
    {
        var treeNode = new TreemapNode(
            current.Id,
            current.Depth,
            current.Text,
            height,
            bounds,
            current.Format,
            parent,
            []);

        var innerBounds = new Rectangle(
            bounds.X,
            bounds.Y + height,
            bounds.Width,
            bounds.Height - height
        );
        innerBounds.Inflate(-(current.Format.BorderWidth + 2), -(current.Format.BorderWidth + 2));

        if (filter.Count > 0 && children.Count() > 1)
        {
            var c = children.Where(c => filter.Contains(c.Id)).FirstOrDefault();
            if (c != null)
            {
                var filtered = Layout(c, isSourceOrderDec, layoutAlign, treeNode, c.Children, innerBounds, height, filter);
                treeNode.Nodes.Add(filtered);
                return treeNode;
            }
        }

        var weights = children.Select(a => a.Weight);
        var weightsOrder = !isSourceOrderDec
            ? weights.OrderBy(w => w)
            : weights.OrderByDescending(w => w);

        var childrenRects = treemapGenerator.Layout(
            innerBounds,
            weightsOrder,
            layout: layoutAlign,
            isCheckSorted: false
        );

        treeNode.Nodes.AddRange(childrenRects.Select((r, i) =>
        {
            var a = children.ElementAt(i);
            return Layout(a, isSourceOrderDec, layoutAlign, treeNode, a.Children, r, height, filter);
        }));
        return treeNode;
    }
}
