using System.Drawing;
using Microsoft.Extensions.Options;
using SquarifiedTreeMapForge.Helpers;
using SquarifiedTreeMapShared;

namespace SquarifiedTreeMapForge.Layout;

public sealed class LegendCalculator
{
    const string DEFAULT_LEGEND_FORMAT = "0%";

    readonly LegendSettings _setting = new();

    public LegendCalculator(IOptions<LegendSettings> settingsOp) => LoadSettings(settingsOp.Value);

    double _avgPer = 0;
    bool _isEnabled = true;
    double _totalDistance = 0.2;
    double _totalBrightness = 0.8;

    public void LoadSettings(LegendSettings settings)
    {
        ValidateSettings(settings);

        _setting.MinValue = Math.Min(settings.MinValue, settings.MaxValue);
        _setting.MaxValue = Math.Max(settings.MinValue, settings.MaxValue);
        _setting.MinBrightness = Math.Min(settings.MinBrightness, settings.MaxBrightness);
        _setting.MaxBrightness = Math.Max(settings.MinBrightness, settings.MaxBrightness);
        _setting.HuePositive = settings.HuePositive;
        _setting.HueNegative = settings.HueNegative;
        _setting.Saturation = settings.Saturation;
        _setting.Margin = settings.Margin;
        _setting.StepCount = settings.StepCount;
        _setting.LegendFormat = string.IsNullOrEmpty(settings.LegendFormat) ? DEFAULT_LEGEND_FORMAT : settings.LegendFormat;
        _setting.IsShowLegend = settings.IsShowLegend;
        _setting.IsOrderAsc = settings.IsOrderAsc;
        _setting.IsShowPlusSign = settings.IsShowPlusSign;
        _setting.Width = settings.Width;
        _setting.Height = settings.Height;

        _avgPer = (_setting.MinValue + _setting.MaxValue) / 2;
        _totalDistance = _setting.MaxValue - _setting.MinValue;
        _totalBrightness = _setting.MaxBrightness - _setting.MinBrightness;
        _isEnabled = _setting.MinValue != _setting.MaxValue && _setting.MinBrightness != _setting.MaxBrightness;
    }

    static void ValidateSettings(LegendSettings settings)
    {
        if (settings.StepCount <= 0|| settings.StepCount > 32)
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
    }

    public Color GetPercentageColor(double per)
        => ColorHelper.HslToRgb(
            _avgPer <= per ? _setting.HuePositive : _setting.HueNegative,
            _setting.Saturation,
            ClampBrightness(per)
        );

    double ClampBrightness(double per)
    {
        if (!_isEnabled) { return _setting.MinBrightness; }

        var normalized = Math.Abs(per - _avgPer) / _totalDistance;
        if (normalized <= 0) { return _setting.MinBrightness; }
        if (normalized >= 1 || double.IsInfinity(normalized)) { return _setting.MaxBrightness; }

        var brightness = _setting.MinBrightness + _totalBrightness * normalized;
        return Math.Clamp(brightness, _setting.MinBrightness, _setting.MaxBrightness);
    }

    public Legend[] GenerateLegends(Rectangle bounds)
    {
        if (!_setting.IsShowLegend) { return []; }

        var legendBounds = new Rectangle(
            bounds.Width - _setting.Width - _setting.Height,
            _setting.Margin,
            _setting.Width,
            _setting.Height);

        var perStep = CalculateSteps(
            (int)Math.Round(_setting.MinValue * 100),
            (int)Math.Round(_setting.MaxValue * 100),
            _setting.StepCount
        );

        var perStepOrdered = _setting.IsOrderAsc
            ? perStep.OrderBy(p => p) : perStep.OrderByDescending(p => p);

        var infrate = _setting.Margin * -2;
        var x = legendBounds.X;
        double cumulativeError = 0;
        var legends = perStepOrdered
            .Select(s =>
            {
                var exactW = (legendBounds.Width + cumulativeError) / _setting.StepCount;
                var w = (int)Math.Round(exactW);
                cumulativeError = exactW - w;

                var per = (s / 100.0);
                var r = new Rectangle(x, legendBounds.Y, w - _setting.Margin, legendBounds.Height);
                x += w;
                var plusSign = _setting.IsShowPlusSign ? "+" : "";
                var mark = (s == 0 ? "" : (s > 0 ? plusSign : "-"));
                var text = $"{mark}{Math.Abs(per).ToString(_setting.LegendFormat)}";

                var textArea = r;
                textArea.Inflate(infrate, infrate);
                return new Legend(r, GetPercentageColor(per), text, textArea);
            });
        return [.. legends];
    }

    static int[] CalculateSteps(int start, int end, int numberOfSteps)
    {
        if (numberOfSteps <= 1) { return [start]; }
        var steps = new int[numberOfSteps];
        var stepSize = (end - start) / (double)(numberOfSteps - 1);

        steps[0] = start;
        steps[numberOfSteps - 1] = end;
        for (int i = 1; i < numberOfSteps - 1; i++)
        {
            steps[i] = (int)Math.Round(start + i * stepSize);
        }
        return steps;
    }
}
