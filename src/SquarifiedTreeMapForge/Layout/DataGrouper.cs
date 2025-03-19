using SquarifiedTreemapShared;

namespace SquarifiedTreemapForge.Layout;

/// <summary>Aggregates data and prepares seeds for generating a treemap.</summary>
public sealed class DataGrouper<T>
{
    /// <summary>Creates the root seed for the treemap.</summary>
    public static Seed<T> CreateSeed(
        IEnumerable<T> sources,
        TreemapLayoutSettings settings,
        TreemapSettings treemapSettings,
        DataGroupPreparer<T> preparer)
    {
        ArgumentNullException.ThrowIfNull(sources);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(treemapSettings);

        var format = new NodeFormat(
            treemapSettings.ForeColor,
            treemapSettings.BackColor,
            preparer.GroupBorderWidths?.FirstOrDefault() ?? 1,
            settings.BorderColor
        );

        int id = 1;
        var children = new List<Seed<T>>();
        AddRangeChildren(children, sources, settings, preparer, ref id, settings.MaxDepth);

        return new Seed<T>(
            0, -2, settings.TitleText,
            children.Sum(a => a.Weight),
            sources, format, children: children);
    }

    static void AddRangeChildren(
        List<Seed<T>> children,
        IEnumerable<T> sources,
        TreemapLayoutSettings settings,
        DataGroupPreparer<T> preparer,
        ref int id,
        int maxDepth,
        int depth = -1,
        Seed<T>? parent = null)
    {
        if (!sources.Any() || maxDepth < depth) return;
        if (depth >= (preparer.GroupProperties?.Length ?? 0)) return;

        var grouped = sources
            .GroupBy(x => preparer.GetGroupKey(x, depth))
            .Where(g => !string.IsNullOrEmpty(g.Key));
        var dimensions = !settings.IsSourceOrderDec
            ? grouped.OrderBy(g => g.Sum(x => preparer.GetWeight(x)))
            : grouped.OrderByDescending(g => g.Sum(x => preparer.GetWeight(x)));

        foreach (var g in dimensions)
        {
            var seed = CreateSeed(g, settings, preparer, ref id, depth, parent);
            AddRangeChildren(seed.Children, g, settings, preparer, ref id, maxDepth, depth + 1, seed);
            children.Add(seed);
        }
    }

    static Seed<T> CreateSeed(
        IGrouping<string, T> group,
        TreemapLayoutSettings settings,
        DataGroupPreparer<T> preparer,
        ref int id,
        int depth,
        Seed<T>? parent)
    {
        var text = preparer.GetGroupText(group.Key, group);
        var weight = group.Sum(preparer.GetWeight);

        var format = new NodeFormat(
            settings.ForeColor,
            preparer.GetColor(group),
            preparer.GetBorderWidth(depth),
            settings.BorderColor
        );

        return new Seed<T>(id++, depth, text, weight, [.. group], format, parent);
    }
}
