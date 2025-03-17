# SquarifiedTreeMapLayers

SquarifiedTreeMapLayers is a library for generating and rendering treemap layouts. This solution provides various functionalities related to treemaps, including layout generation, color calculation, and legend generation.

## Features

- Treemap layout generation
- Customizable color calculation
- Legend generation and display
- Compatible with .NET 8

## Installation

Clone this repository and open it in Visual Studio 2022.

## Usage

### Generating a Treemap

The following code demonstrates how to generate and render a treemap layout.

```
using SquarifiedTreeMapForge.WinForms; 
using SquarifiedTreeMapShared; 
using System.Drawing;

var settings = new TreeMapSettings { TitleText = "Sample TreeMap", TitleFontName = "Arial", TitleFontSize = 12, ForeColor = Color.Black, BackColor = Color.White, HighlightColor = Color.Red, HighlightWidth = 2, Margin = 1 };

var layoutSettings = new TreeMapLayoutSettings { WeightColumn = "Value", AggregateColumns = new string[] { "Category" }, AggregateColumnFormats = new string[] { "0.0" }, AggregateColumnBorderWidths = new int[] { 1 }, ForeColor = Color.Black, NodeFontName = "Arial", NodeFontSize = 10, BorderColor = Color.Gray, MaxDepth = 5, IsSourceOrderDec = true, LayoutAlign = LayoutAlign.LeftTop };

var renderer = new GdiRenderer(); 
var coordinator = new LayoutCoordinator<YourDataType>(new LayoutGenerator<YourDataType>()); 
var driver = new TreeMapDriver<YourDataType>(settings, layoutSettings, coordinator, renderer);
driver.SetDataSource(yourDataSource); 
driver.Render(800, 600).Save("treemap.png", System.Drawing.Imaging.ImageFormat.Png);
```

### Generating Legends

The following code demonstrates how to generate legends.

```
using SquarifiedTreeMapForge.Layout; 
using SquarifiedTreeMapShared; 
using System.Drawing;

var legendSettings = new LegendSettings { MinValue = -0.1, MaxValue = 0.1, MinBrightness = 0.08, MaxBrightness = 0.78, HuePositive = 205.0, HueNegative = 2.0, Saturation = 0.9, Steps = 7, Margin = 1, IsOrderAsc = false, IsShowLegend = true, IsShowPlusSign = true, LegendFormat = "0%" };
var legendCalculator = new LegendCalculator(Options.Create(legendSettings)); var legends = legendCalculator.GenerateLegends(new Rectangle(0, 0, 800, 50));
```

## Contributing

Contributions are welcome. Feel free to report bugs, suggest new features, or submit pull requests.

1. Fork this repository.
2. Create a feature branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -am 'Add some feature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Create a new Pull Request.

## License

This project is licensed under the MIT License. 