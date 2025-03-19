using System.Drawing;
using SquarifiedTreemapForge.Helpers;
using SquarifiedTreemapShared;

namespace SquarifiedTreemapForge.Layout;

/// <summary>Prepares data for generating a treemap.</summary>
public sealed class DataGroupPreparer<T>
{
    public const int MINIMUM_SIZE = 2;
    TreemapLayoutSettings? _settings;

    public void Initialize(
        TreemapLayoutSettings settings,
        Func<string, IEnumerable<T>, string>? funcNodeText,
        Func<IEnumerable<T>, Color>? funcNodeColor)
    {
        _settings = settings;
        FuncNodeText = funcNodeText;
        FuncNodeColor = funcNodeColor;

        WeightProperty = Cache<T>.Properties
            .FirstOrDefault(p => p.Name.Equals(_settings.WeightColumn, StringComparison.OrdinalIgnoreCase))
            ?? throw new KeyNotFoundException($"Weight column '{_settings.WeightColumn}' not found.");
        GroupProperties = [.. _settings.GroupColumns.Select(c =>
            Cache<T>.Properties.FirstOrDefault(p => p.Name.Equals(c, StringComparison.OrdinalIgnoreCase))
            ?? throw new KeyNotFoundException($"Group column '{c}' not found."))];
        GroupColumnFormats = InitializeGroupColumnFormats(_settings.GroupColumnFormats, _settings.GroupColumns.Length);
        GroupBorderWidths = InitializeGroupBorderWidth(_settings.GroupBorderWidths, _settings.GroupColumns.Length);
    }

    static int[] InitializeGroupBorderWidth(int[]? groupBorderWidth, int length)
    {
        if (groupBorderWidth == null || groupBorderWidth.Length == 0)
        {
            return [.. Enumerable.Repeat(1, length).Select((v, i) => i < 2 ? MINIMUM_SIZE * (2 - i) : v)];
        }

        if (groupBorderWidth.Length > length)
        {
            return [.. groupBorderWidth.Take(length)];
        }

        groupBorderWidth = [.. groupBorderWidth.Select(w => w < 0 ? 1 : w)];
        return groupBorderWidth.Length == length
            ? groupBorderWidth
            : [.. groupBorderWidth, .. Enumerable.Repeat(1, length - groupBorderWidth.Length)];
    }

    static string[] InitializeGroupColumnFormats(string[]? formats, int length)
    {
        if (formats == null || formats.Length == 0) { return new string[length]; }
        if (formats.Length == length) { return formats; }
        if (formats.Length > length) { return [.. formats.Take(length)]; }
        return [.. formats.Concat(Enumerable.Repeat(string.Empty, length - formats.Length))];
    }

    public PropCache? WeightProperty { get; private set; }
    public PropCache[]? GroupProperties { get; private set; }
    public string[]? GroupColumnFormats { get; private set; }
    public int[]? GroupBorderWidths { get; private set; }

    public Func<string, IEnumerable<T>, string>? FuncNodeText { get; private set; }
    public Func<IEnumerable<T>, Color>? FuncNodeColor { get; private set; }

    public string GetGroupText(string aggregateValue, IEnumerable<T> sources)
        => FuncNodeText?.Invoke(aggregateValue, sources) ?? aggregateValue;

    public Color GetColor(IEnumerable<T> sources)
        => FuncNodeColor?.Invoke(sources) ?? GenerateRandomColor();

    static Color GenerateRandomColor()
    {
        return Color.FromArgb(
            red: Random.Shared.Next(64, 192),
            green: Random.Shared.Next(64, 192),
            blue: Random.Shared.Next(64, 192));
    }

    public int GetBorderWidth(int depth = 0)
    {
        if (GroupBorderWidths == null || depth < 0 || depth >= GroupBorderWidths.Length)
        {
            return 1;
        }
        var w = GroupBorderWidths[depth];
        return w > 0 ? w : 1;
    }

    public string GetGroupKey(T x, int depth = 0)
    {
        if (GroupProperties == null || GroupColumnFormats == null) { return ""; }
        if (x == null || _settings?.GroupColumns == null) { return ""; }
        if (depth < 0) return _settings.RootNodeTitle;
        if (depth >= GroupProperties.Length) { return ""; }

        var value = GroupProperties[depth].GetValue(x);
        if (value == null) { return ""; }

        if (GroupColumnFormats.Length == 0 || depth >= GroupColumnFormats.Length)
            return value.ToString() ?? "";

        var fmt = GroupColumnFormats[depth];
        return value switch
        {
            int i => i.ToString(fmt),
            double d => d.ToString(fmt),
            float f => f.ToString(fmt),
            decimal e => e.ToString(fmt),
            long l => l.ToString(fmt),
            DateTime dt => dt.ToString(fmt),
            DateOnly dto => dto.ToString(fmt),
            TimeOnly tio => tio.ToString(fmt),
            _ => value.ToString() ?? ""
        };
    }

    public double GetWeight(T x)
    {
        if (x == null || WeightProperty == null) return 0;
        return WeightProperty.GetValue(x) switch
        {
            int i => i,
            double d => d,
            float f => f,
            decimal e => (double)e,
            long l => l,
            _ => 0
        };
    }
}
