using System.Drawing;

namespace SquarifiedTreeMapShared;

public record TreeMapSettings
{
    public string TitleFontFamily { get; init; } = "Alial";
    public float TitleFontSize { get; init; } = 12;
    public bool IsTitleFontBold { get; init; } = false;

    public int Margin { get; init; } = 1;
    public Color ForeColor { get; init; } = Color.Black;
    public Color BackColor { get; init; } = Color.White;
    public Color HighlightColor { get; init; } = Color.White;
    public int HighlightWidth { get; init; } = 2;

    public string NodeFontFamily { get; init; } = "Alial";
    public float NodeFontSize { get; init; } = 9.5F;
    public bool IsNodeFontBold { get; init; } = false;

    public string LegendFontFamily { get; init; } = "Alial";
    public float LegendFontSize { get; init; } = 7.5F;
    public bool IsLegendFontBold { get; init; } = false;
}

public record TreeMapLayoutSettings
{
    public string TitleText { get; init; } = "Title";
    public string RootNodeTitle { get; init; } = "Root";
    public string WeightColumn { get; init; } = "";
    public string[] GroupColumns { get; init; } = [];
    public string[] GroupColumnFormats { get; init; } = [];
    public int[] GroupBorderWidths { get; init; } = [];
    public Color ForeColor { get; init; } = Color.Black;
    public Color BorderColor { get; init; } = Color.DimGray;
    public int MaxDepth { get; init; } = 32;
    public bool IsSourceOrderDec { get; init; } = true;
    public LayoutAlign LayoutAlign { get; init; } = LayoutAlign.LeftTop;
}

public record LegendSettings
{
    public int Width { get; init; } = 250;
    public int Height { get; init; } = 20;
    public double MinPer { get; init; } = -0.1;
    public double MaxPer { get; init; } = 0.1;
    public double MinBrightness { get; init; } = 0.08;
    public double MaxBrightness { get; init; } = 0.78;
    public double HuePositive { get; init; } = 205.0;
    public double HueNegative { get; init; } = 2.0;
    public double Saturation { get; init; } = 0.9;
    public int StepCount { get; init; } = 7;
    public int Margin { get; init; } = 1;
    public bool IsOrderAsc { get; init; } = false;
    public string LegendFormat { get; init; } = "0%";
    public bool IsShowLegend { get; init; } = true;
    public bool IsShowPlusSign { get; init; } = true;

    const string DEFAULT_LEGEND_FORMAT = "0%";

    public LegendSettings With(LegendSettings settings)
    {
        if (settings.StepCount <= 0 || settings.StepCount > 32)
        {
            throw new ArgumentException("Steps must be must be between 0 and 32.");
        }
        if (settings.Margin < 0)
        {
            throw new ArgumentException("Margin must be non-negative.");
        }
        if (settings.MinBrightness < 0 || settings.MinBrightness > 1 || settings.MaxBrightness < 0 || settings.MaxBrightness > 1)
        {
            throw new ArgumentException("Brightness values must be between 0 and 1.");
        }
        if (settings.Saturation < 0 || settings.Saturation > 1)
        {
            throw new ArgumentException("Saturation must be between 0 and 1.");
        }
        if (settings.HuePositive < 0 || settings.HuePositive > 360)
        {
            throw new ArgumentException("Saturation must be between 0 and 360.");
        }
        if (settings.HueNegative < 0 || settings.HueNegative > 360)
        {
            throw new ArgumentException("Saturation must be between 0 and 360.");
        }

        return this with
        {
            MinPer = Math.Min(settings.MinPer, settings.MaxPer),
            MaxPer = Math.Max(settings.MinPer, settings.MaxPer),
            MinBrightness = Math.Min(settings.MinBrightness, settings.MaxBrightness),
            MaxBrightness = Math.Max(settings.MinBrightness, settings.MaxBrightness),
            HuePositive = settings.HuePositive,
            HueNegative = settings.HueNegative,
            Saturation = settings.Saturation,
            Margin = settings.Margin,
            StepCount = settings.StepCount,
            LegendFormat = string.IsNullOrEmpty(settings.LegendFormat) ? DEFAULT_LEGEND_FORMAT : settings.LegendFormat,
            IsShowLegend = settings.IsShowLegend,
            IsOrderAsc = settings.IsOrderAsc,
            IsShowPlusSign = settings.IsShowPlusSign,
            Width = settings.Width,
            Height = settings.Height,
        };
    }
}