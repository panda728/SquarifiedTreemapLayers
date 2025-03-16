using System.Drawing;

namespace SquarifiedTreeMapShared;

public sealed class TreeMapSettings
{
    public string TitleFontFamily { get; set; } = "Alial";
    public float TitleFontSize { get; set; } = 12;
    public bool IsTitleFontBold { get; set; } = false;

    public int Margin { get; set; } = 1;
    public Color ForeColor { get; set; } = Color.Black;
    public Color BackColor { get; set; } = Color.White;
    public Color HighlightColor { get; set; } = Color.White;
    public int HighlightWidth { get; set; } = 2;

    public string NodeFontFamily { get; set; } = "Alial";
    public float NodeFontSize { get; set; } = 9.5F;
    public bool IsNodeFontBold { get; set; } = false;

    public string LegendFontFamily { get; set; } = "Alial";
    public float LegendFontSize { get; set; } = 7.5F;
    public bool IsLegendFontBold { get; set; } = false;
}

public sealed class TreeMapLayoutSettings
{
    public string TitleText { get; set; } = "Title";
    public string RootNodeTitle { get; set; } = "Root";
    public string WeightColumn { get; set; } = "";
    public string[] AggregateColumns { get; set; } = [];
    public string[] AggregateColumnFormats { get; set; } = [];
    public int[] AggregateColumnBorderWidths { get; set; } = [];
    public Color ForeColor { get; set; } = Color.Black;
    public Color BorderColor { get; set; } = Color.DimGray;
    public int MaxDepth { get; set; } = 32;
    public bool IsSourceOrderDec { get; set; } = true;
    public LayoutAlign LayoutAlign { get; set; } = LayoutAlign.LeftTop;
}

public sealed class LegendSettings
{
    public int Width { get; set; } = 250;
    public int Height { get; set; } = 20;
    public double MinValue { get; set; } = -0.1;
    public double MaxValue { get; set; } = 0.1;
    public double MinBrightness { get; set; } = 0.08;
    public double MaxBrightness { get; set; } = 0.78;
    public double HuePositive { get; set; } = 205.0;
    public double HueNegative { get; set; } = 2.0;
    public double Saturation { get; set; } = 0.9;
    public int StepCount { get; set; } = 7;
    public int Margin { get; set; } = 1;
    public bool IsOrderAsc { get; set; } = false;
    public string LegendFormat { get; set; } = "0%";
    public bool IsShowLegend { get; set; } = true;
    public bool IsShowPlusSign { get; set; } = true;
}