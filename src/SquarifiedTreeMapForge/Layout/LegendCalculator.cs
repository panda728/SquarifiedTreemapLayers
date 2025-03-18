using System.Drawing;
using Microsoft.Extensions.Options;
using SquarifiedTreeMapForge.Helpers;
using SquarifiedTreeMapShared;

namespace SquarifiedTreeMapForge.Layout;

public sealed class LegendCalculator
{
    public LegendCalculator(IOptions<LegendSettings> settingsOp) => LoadSettings(settingsOp.Value);

    LegendSettings Settings { get; set; } = new();
    LegendState State { get; set; } = new();

    public void LoadSettings(LegendSettings settings)
    {
        Settings = Settings.With(settings);
        State = State.With(Settings);
    }

    public Color GetPercentageColor(double per)
        => ColorHelper.HslToRgb(
            State.AvgPer <= per ? Settings.HuePositive : Settings.HueNegative,
            Settings.Saturation,
            ClampBrightness(per)
        );

    double ClampBrightness(double per)
    {
        if (!State.IsEnabled) { return Settings.MinBrightness; }

        var normalized = Math.Abs(per - State.AvgPer) / State.TotalDistance;
        if (normalized <= 0) { return Settings.MinBrightness; }
        if (normalized >= 1 || double.IsInfinity(normalized)) { return Settings.MaxBrightness; }

        var brightness = Settings.MinBrightness + State.TotalBrightness * normalized;
        return Math.Clamp(brightness, Settings.MinBrightness, Settings.MaxBrightness);
    }

    public Legend[] GenerateLegends(Rectangle bounds)
    {
        if (!Settings.IsShowLegend) { return []; }

        var legendBounds = new Rectangle(
            bounds.Width - Settings.Width - Settings.Height,
            Settings.Margin,
            Settings.Width,
            Settings.Height);

        var perStep = CalculateSteps(
            (int)Math.Round(Settings.MinPer * 100),
            (int)Math.Round(Settings.MaxPer * 100),
            Settings.StepCount
        );

        var perStepOrdered = Settings.IsOrderAsc
            ? perStep.OrderBy(p => p) : perStep.OrderByDescending(p => p);

        var infrate = Settings.Margin * -2;
        var x = legendBounds.X;
        double cumulativeError = 0.0;
        var legends = perStepOrdered
            .Select(s =>
            {
                var exactW = (legendBounds.Width + cumulativeError) / Settings.StepCount;
                var w = (int)Math.Round(exactW);
                cumulativeError = exactW - w;

                var per = (s / 100.0);
                var r = new Rectangle(x, legendBounds.Y, w - Settings.Margin, legendBounds.Height);
                x += w;
                var plusSign = Settings.IsShowPlusSign ? "+" : "";
                var mark = (s == 0 ? "" : (s > 0 ? plusSign : "-"));
                var text = $"{mark}{Math.Abs(per).ToString(Settings.LegendFormat)}";

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

    record LegendState(double AvgPer = 0, double TotalDistance = 0.2, double TotalBrightness = 0.8, bool IsEnabled = true)
    {
        public LegendState With(LegendSettings settings)
            => this with
            {
                AvgPer = (settings.MinPer + settings.MaxPer) / 2,
                TotalDistance = settings.MaxPer - settings.MinPer,
                TotalBrightness = settings.MaxBrightness - settings.MinBrightness,
                IsEnabled = settings.MinPer != settings.MaxPer && settings.MinBrightness != settings.MaxBrightness,
            };
    };
}
