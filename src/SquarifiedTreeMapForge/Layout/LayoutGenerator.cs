using System.Drawing;
using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapForge.Layout;

/// <summary>Manages the layout of a treemap.</summary>
public sealed class LayoutGenerator<T>(ITreemapGenerator treemapGenerator)
{
    const int MinimumNodeSize = 2;
    const int MaximumNodeSize = int.MaxValue / 2;

    /// <summary>Recursively calculates the layout of the treemap nodes.</summary>
    public TreemapNode Layout(
        Seed<T> current,
        bool isSourceOrderDec,
        LayoutAlign layoutAlign,
        TreemapNode? parent,
        IEnumerable<Seed<T>> children,
        Rectangle bounds,
        int height,
        HashSet<int> filter,
        int displayDepthMax = 1024,
        int currentDisplayDepth = 0)
    {
        if (currentDisplayDepth > displayDepthMax)
        {
            return new TreemapNode(
                current.Id,
                current.Depth,
                current.Text,
                height,
                bounds,
                current.Format,
                parent,
                []);
        }

        var outerBounds = bounds;
        if (current.Format.ExplodeGap > 0)
        {
            var gap = -current.Format.ExplodeGap;
            outerBounds.Inflate(gap, gap);
        }

        var treeNode = new TreemapNode(
            current.Id,
            current.Depth,
            current.Text,
            height,
            outerBounds,
            current.Format,
            parent,
            []);

        var innerBounds = new Rectangle(
            outerBounds.X,
            outerBounds.Y + height,
            outerBounds.Width,
            outerBounds.Height - height
        );
        innerBounds.Inflate(-(current.Format.BorderWidth + 2), -(current.Format.BorderWidth + 2));

        if (IsOutOfRange(innerBounds)) { return treeNode; }

        if (filter.Count > 0 && children.Count() > 1)
        {
            var c = children.FirstOrDefault(c => filter.Contains(c.Id));
            if (c != null)
            {
                var filtered = Layout(
                    c,
                    isSourceOrderDec,
                    layoutAlign,
                    treeNode,
                    c.Children,
                    innerBounds,
                    height,
                    filter,
                    displayDepthMax,
                    currentDisplayDepth);
                treeNode.Nodes.Add(filtered);
                return treeNode;
            }
        }

        var weights = children.Select(a => a.Weight);
        var weightsOrder = !isSourceOrderDec
            ? weights.OrderBy(w => w)
            : weights.OrderByDescending(w => w);

        var weightsSum = weights.Sum();
        if (weightsSum == 0) {  return treeNode; }

        var childrenRects = treemapGenerator.Layout(
            innerBounds,
            weightsOrder,
            layout: layoutAlign,
            isCheckSorted: false
        );

        treeNode.Nodes.AddRange(childrenRects.Select((r, i) =>
        {
            var a = children.ElementAt(i);
            return Layout(
                a,
                isSourceOrderDec,
                layoutAlign,
                treeNode,
                a.Children,
                r,
                height,
                filter,
                displayDepthMax,
                currentDisplayDepth + 1);
        }));
        return treeNode;
    }

    static bool IsOutOfRange(Rectangle bounds) 
        => IsOutOfRange(bounds.X)
        || IsOutOfRange(bounds.Y)
        || IsOutOfRange(bounds.Width) 
        || IsOutOfRange(bounds.Height);

    static bool IsOutOfRange(int i) => i < MinimumNodeSize || i > MaximumNodeSize;
}
