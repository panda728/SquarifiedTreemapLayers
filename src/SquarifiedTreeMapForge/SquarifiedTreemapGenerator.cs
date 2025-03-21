using System.Drawing;
using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapForge;

/// <summary>Generates a squarified treemap based on the given weights and canvas area.</summary>
public class SquarifiedTreemapGenerator : ITreemapGenerator
{
    const int MINIMUM_SIZE = 6;

    /// <summary>Places rectangles based on the given list of weights within the specified area.</summary>
    public IEnumerable<Rectangle> Layout(
        Rectangle bounds,
        IEnumerable<double> weights,
        LayoutAlign layout = LayoutAlign.LeftTop,
        int minimumSize = MINIMUM_SIZE,
        bool isCheckSorted = true)
    {
        /*
        *  Tree Visualization with Tree-Maps: 2-d Space-Filling Approach  Ben Shneiderman  University of Maryland
        * https://www.ifs.tuwien.ac.at/~mlanzenberger/teaching/ps/ws08/stuff/p92-shneiderman.pdf
        * 
        * Squarified Treemaps Mark Bruls, Kees Huizing, and Jarke J. van Wijk
        * https://vanwijk.win.tue.nl/stm.pdf
        * 
        * Extension of Network Traffic Visualization System Using Treemap and Edge Bundling Ryo Akiyoshi, Yoshihiro Okada
        * https://www.ipsj-kyushu.jp/page/ronbun/hinokuni/1005/2B/2B-4.pdf
        */
        if (bounds == Rectangle.Empty || !weights.Any()) { yield break; }
        if (bounds.Width <= minimumSize || bounds.Height <= minimumSize) { yield break; }

        var weightsArray = weights is double[] array ? array : [.. weights];
        if (isCheckSorted && !IsSorted(weightsArray.AsSpan()))
        {
            throw new ArgumentException("The list of weights must be sorted in descending order.");
        }

        var cumulativeErrorX = 0.0;
        var cumulativeErrorY = 0.0;
        var isLayoutSwitchedX = false;
        var isLayoutSwitchedY = false;

        var filledCount = 0;
        var currentBounds = bounds;
        var count = weightsArray.Length;
        while (filledCount < count)
        {
            var (remainingBounds, rectangles) = CalculateOptimalSquareFillPattern(
                currentBounds, weightsArray, filledCount, layout,
                ref cumulativeErrorX, ref cumulativeErrorY,
                ref isLayoutSwitchedX, ref isLayoutSwitchedY);

            if (rectangles == null || rectangles.Length == 0) { break; }
            foreach (var rectangle in rectangles)
            {
                yield return rectangle;
            }

            filledCount += rectangles.Length;
            currentBounds = remainingBounds;
        }
    }

    static bool IsSorted<T>(Span<T> span) where T : IComparable<T>
    {
        if (span.Length <= 1)
        {
            return true;
        }

        bool isAscending = true;
        bool isDescending = true;

        for (int i = 1; i < span.Length; i++)
        {
            if (span[i - 1].CompareTo(span[i]) > 0)
            {
                isAscending = false;
            }
            if (span[i - 1].CompareTo(span[i]) < 0)
            {
                isDescending = false;
            }
        }
        return isAscending || isDescending;
    }

    static (Rectangle remainingBounds, Rectangle[] rectangles) CalculateOptimalSquareFillPattern(
       Rectangle bounds, Span<double> weights, int startIndex, LayoutAlign layout,
       ref double cumulativeErrorX, ref double cumulativeErrorY,
       ref bool isLayoutSwitchedX, ref bool isLayoutSwitchedY)
    {
        if (startIndex >= weights.Length) { return (bounds, Array.Empty<Rectangle>()); }

        var isHorizontalSplit = bounds.Width >= bounds.Height;

        var (minorSides, longSide, cumulativeError) = MapRectangle(
            bounds.Size, weights, startIndex, isHorizontalSplit ? cumulativeErrorX : cumulativeErrorY);

        if (isHorizontalSplit)
        {
            cumulativeErrorX = cumulativeError;
            isLayoutSwitchedX = !isLayoutSwitchedX;
        }
        else
        {
            cumulativeErrorY = cumulativeError;
            isLayoutSwitchedY = !isLayoutSwitchedY;
        }

        var isLayoutSwitched = isHorizontalSplit ? isLayoutSwitchedX : isLayoutSwitchedY;
        var remainingBounds = CalculateRemaining(bounds, longSide, isHorizontalSplit, isLayoutSwitched, layout);

        var currentOffset = 0;
        var results = new Rectangle[minorSides.Length];
        for (int i = 0; i < minorSides.Length; i++)
        {
            results[i] = BuildRectangle(bounds, longSide, minorSides[i], currentOffset, isHorizontalSplit, isLayoutSwitched, layout);
            currentOffset += isHorizontalSplit ? results[i].Height : results[i].Width;
        }
        return (remainingBounds, results);
    }

    static bool IsAlignLeft(LayoutAlign layout, bool isLayoutSwitched)
        => layout switch
        {
            LayoutAlign.LeftTop => true,
            LayoutAlign.LeftBottom => true,
            LayoutAlign.Alternating => isLayoutSwitched,
            _ => false,
        };

    static bool IsAlignTop(LayoutAlign layout, bool isLayoutSwitched)
        => layout switch
        {
            LayoutAlign.LeftTop => true,
            LayoutAlign.RightTop => true,
            LayoutAlign.Alternating => isLayoutSwitched,
            _ => false,
        };

    static Rectangle BuildRectangle(
        Rectangle bounds, int longSide, int minorSide, int currentOffset,
        bool isHorizontalSplit, bool isLayoutSwitched, LayoutAlign layout)
    {
        int x = bounds.X;
        int y = bounds.Y;
        if (isHorizontalSplit)
        {
            x += IsAlignLeft(layout, isLayoutSwitched) ? 0 : bounds.Width - longSide;
            y += currentOffset;
        }
        else
        {
            x += currentOffset;
            y += IsAlignTop(layout, isLayoutSwitched) ? 0 : bounds.Height - longSide;
        }
        return isHorizontalSplit
            ? new Rectangle(x, y, longSide, minorSide)
            : new Rectangle(x, y, minorSide, longSide);
    }

    static Rectangle CalculateRemaining(
        Rectangle bounds, int longSide,
        bool isHorizontalSplit, bool isLayoutSwitched, LayoutAlign layout)
    {
        return new Rectangle(
            bounds.X + (isHorizontalSplit ? (IsAlignLeft(layout, isLayoutSwitched) ? longSide : 0) : 0),
            bounds.Y + (isHorizontalSplit ? 0 : (IsAlignTop(layout, isLayoutSwitched) ? longSide : 0)),
            bounds.Width - (isHorizontalSplit ? longSide : 0),
            bounds.Height - (isHorizontalSplit ? 0 : longSide));
    }

    static (int[] minorSides, int longSide, double cumulativeError) MapRectangle(
        Size size, Span<double> weights, int startIndex, double cumulativeError)
    {
        var width = Math.Max(size.Width, size.Height);
        var height = Math.Min(size.Width, size.Height);
        var (splitWidth, splitHeights) = FindOptimalSplit(width, height, weights, startIndex, ref cumulativeError);
        return (TruncateHeights(height, splitHeights), splitWidth, cumulativeError);
    }

    static (int splitWidth, double[] splitHeights) FindOptimalSplit(
        int width, int height, Span<double> weights, int startIndex, ref double cumulativeError)
    {
        var totalWeight = 0d;
        foreach (var weight in weights[startIndex..])
        {
            totalWeight += weight;
        }

        SplitCache previous = new(1, 0, 0, []);
        for (int i = startIndex; i < weights.Length; i++)
        {
            var chunkWeights = weights.Slice(startIndex, i - startIndex + 1);
            var subTotalWeight = 0d;
            foreach (var w in chunkWeights)
            {
                subTotalWeight += w;
            }
            var ratio = subTotalWeight / totalWeight;
            var exactSplitWidth = width * ratio;
            var splitWidth = (int)Math.Floor(exactSplitWidth + cumulativeError);
            var splitHeights = new List<double>(chunkWeights.Length);
            foreach (var w in chunkWeights)
            {
                splitHeights.Add(height * (w / subTotalWeight));
            }
            var currentAspect = splitHeights
                .Max(h => 1.0 - (Math.Min(h, splitWidth) / Math.Max(h, splitWidth)));
            if (currentAspect < previous.Aspect || previous.Heights.Count == 0)
            {
                previous = new(currentAspect, exactSplitWidth, splitWidth, splitHeights);
                continue;
            }
            cumulativeError += previous.ExactWidth - previous.Width;
            return (previous.Width, previous.Heights.ToArray());
        }
        var weightsArray = weights[startIndex..].ToArray();
        return (width, weightsArray.Select(w => height * (w / totalWeight)).ToArray());
    }

    static int[] TruncateHeights(int totalHeight, double[] exactHeights)
    {
        if (exactHeights.Length == 0) { return []; }
        var heights = new int[exactHeights.Length];
        var allocatedHeight = 0;
        var cumulativeError = 0.0;
        for (int i = 0; i < exactHeights.Length - 1; i++)
        {
            heights[i] = (int)Math.Round(exactHeights[i] + cumulativeError);
            allocatedHeight += heights[i];
            cumulativeError += exactHeights[i] - heights[i];
        }
        heights[exactHeights.Length - 1] = totalHeight - allocatedHeight;
        return heights;
    }

    record SplitCache(double Aspect, double ExactWidth, int Width, List<double> Heights);
}
