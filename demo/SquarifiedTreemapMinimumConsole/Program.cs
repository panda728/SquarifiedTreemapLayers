﻿using System.Text.Json;
using Microsoft.Extensions.Options;
using SquarifiedTreemapForge;
using SquarifiedTreemapForge.Layout;
using SquarifiedTreemapForge.WinForms;
using SquarifiedTreemapForge.Shared;

var settings = new TreemapSettings();
var layoutSettings = new TreemapLayoutSettings { TitleText = "Visualizing Sales Revenue (Area) and Cost of Goods Sold Ratio (Color)", RootNodeTitle = "Total Sales", WeightColumn = "Weight", GroupColumns = ["Group1", "Group2", "Group3"], GroupBorderWidths = [4, 2], };
var legendSettings = new LegendSettings() { Width = 250, Height = 20, MinPer = 0.73, MaxPer = 1, MinBrightness = 0.2, MaxBrightness = 0.9, HuePositive = 2, HueNegative = 205, Saturation = 0.85, StepCount = 7, Margin = 1, IsOrderAsc = false, LegendFormat = "0%", IsShowLegend = true, IsShowPlusSign = true };

var interactor = new LayoutInteractor<PivotDataSource>(
    new LayoutGenerator<PivotDataSource>(new SquarifiedTreemapGenerator()),
    new DataGroupPreparer<PivotDataSource>(),
    new LegendGenerator()
);

var driver = new TreemapGdiDriver<PivotDataSource>(
    new GdiRenderer(), interactor, Options.Create(settings), Options.Create(layoutSettings), Options.Create(legendSettings))
{
    FuncNodeText = PivotDataSource.GetTitle,
    FuncPercentage = PivotDataSource.GetPercentage
};

var json = File.ReadAllText(@"..\..\..\..\..\sample\sales\sales_data.json");
var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var data = JsonSerializer.Deserialize<IEnumerable<PivotDataSource>>(json, options) ?? [];
driver.Invalidate(data);

driver.Render(1024, 768)
    .Save("treemap.png", System.Drawing.Imaging.ImageFormat.Png);

Console.WriteLine($"Treemap image saved to {new FileInfo("treemap.png").FullName}");
