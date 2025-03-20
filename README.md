# SquarifiedTreemapLayers

SquarifiedTreemapLayers is a library for generating and rendering treemap layouts. This solution provides various functionalities related to treemaps, including layout generation, color calculation, and legend generation.

## Features

- Treemap layout generation
- Customizable color calculation
- Legend generation and display
- Compatible with .NET 8
- TreemapGdiDriver as an entry point for development kits
- `SquarifiedTreemapConsole` as an example console program that supports PNG saving and DataUrl output with given data and settings
- `SquarifiedTreemapWinForms` as an example Windows Forms program that displays data, shows content at the mouse position in the status bar, provides interactive zoom on double-clicking group items, and allows immediate result verification upon changing settings in the configuration screen

### PNG Saving Sample

![Image](https://github.com/user-attachments/assets/c64b0c15-c753-4978-afa0-f73f93567d6a)

### SquarifiedTreemapWinForms Screen

Setting Screen (layout, color, legend)

See: `SquarifiedTreemapLayers\src\SquarifiedTreemapWinForms`

![Image](https://github.com/user-attachments/assets/1db306a3-35a4-4b03-bb63-ea086d812807)

## System Architecture

### SquarifiedTreemapWinForms

The `SquarifiedTreemapWinForms` application is structured as follows:

```mermaid
graph TD 
A[Program] -->|Run| B[FormMain] 
B -->|Controls.Add| C[TreemapControl] 
B -->|SetData| D[TreemapGdiDriver<T>]
D -->|SetData| E[LayoutInteractor<T>] 
E -->|SetData| F[DataGroupPreparer<T>] 
D -->|OnPaint| E
E -->|OnPaint| G[LayoutGenerator<T>] 
G -->|OnPaint| H[SquarifiedTreemapGenerator] 
E -->|OnPaint| I[LegendGenerator]
C --> |Mouse Event| D
D -->|OnPaint| J[GdiRenderer] 
```
- **Program**: The entry point of the application. It builds the host and runs the `FormMain`.
- **FormMain**: The main Windows form that provides the user interface.
- **TreemapGdiDriver<T>**: Responsible for rendering the treemap.
- **GdiRenderer**: Uses GDI+ for rendering.
- **LayoutInteractor<T>**, **LayoutGenerator<T>**, **DataGroupPreparer<T>**, **LegendGenerator**, **SquarifiedTreemapGenerator**: Handle various aspects of treemap layout, data preparation, and legend calculation.

This architecture ensures a modular and maintainable codebase, allowing for easy extension and customization of the treemap functionalities.

### SquarifiedTreemapConsole

The `SquarifiedTreemapConsole` application is structured as follows:

```mermaid
graph TD 
A[Program] -->|Run| B[Exporter] 
B -->|SetData| C[LayoutInteractor<T>] 
C -->|SetData| D[DataGroupPreparer<T>]
B --> |Render| C
C -->|Render| E[LayoutGenerator<T>] 
E -->|Render| F[SquarifiedTreemapGenerator] 
C -->|Render| G[LegendGenerator]
B -->|Render| H[GdiRenderer] 
```

- **Program**: The entry point of the application. It parses command-line arguments, builds the host, and runs the `Exporter`.
- **Exporter**: Handles the export process, including reading data, generating the treemap, and saving the output.
- **GdiRenderer**: Uses GDI+ for rendering.
- **LayoutInteractor<T>**, **LayoutGenerator<T>**, **DataGroupPreparer<T>**, **LegendGenerator**, **SquarifiedTreemapGenerator**: Handle various aspects of treemap layout, data preparation, and legend calculation.

This architecture ensures a modular and maintainable codebase, allowing for easy extension and customization of the treemap functionalities.

## Installation

To install and run the `SquarifiedTreemapConsole` application, follow these steps:

1. Clone the repository:

```cmd
git clone https://github.com/yourusername/SquarifiedTreemapLayers.git
cd SquarifiedTreemapLayers/SquarifiedTreemapConsole
```

2. Build the project in Release mode to generate the executable:

```cmd
dotnet publish -c Release -r win-x64 --self-contained
```

This command will create a self-contained executable in the `bin\Release\net8.0\win-x64\publish` directory.

3. Navigate to the publish directory and run the executable:

```cmd
cd bin\Release\net8.0\win-x64\publish
SquarifiedTreemapConsole.exe <width> <height> <datapath> [pngfullpath]
```

Replace `<width>`, `<height>`, `<datapath>`, and `[pngfullpath]` with the appropriate values for your use case.

### Sample Data and Configuration

Sample data and configuration examples can be found in the `SquarifiedTreemapLayers\sample\sales` folder.  

## Usage

### With Minimal Setup

The following code demonstrates how to generate and render a treemap layout with minimal setup. The data file used in this example is `sample\sales\sales_data.json`.

```csharp
using System.Text.Json;
using Microsoft.Extensions.Options;
using SquarifiedTreemapForge;
using SquarifiedTreemapForge.Layout;
using SquarifiedTreemapForge.WinForms;
using SquarifiedTreemapInteractor;
using SquarifiedTreemapShared;

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
```

### Dependency injection configuration examples.

The following code examples using Dependency Injection (DI).  

```csharp
Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<TreemapSettings>(
            hostContext.Configuration.GetSection("TreemapSettings"));
        services.Configure<TreemapLayoutSettings>(
            hostContext.Configuration.GetSection("TreemapLayoutSettings"));
        services.Configure<LegendSettings>(
            hostContext.Configuration.GetSection("LegendSettings"));

        // Settings
        services.AddTransient<TreemapSettings>();
        services.AddTransient<TreemapLayoutSettings>();
        services.AddTransient<LegendSettings>();

        // this project
        services.AddTransient<FormMain>();

        // SquarifiedTreemapForge.WinForms.csproj
        services.AddTransient<TreemapGdiDriver<PivotDataSource>>();
        services.AddTransient<GdiRenderer>();

        // SquarifiedTreemapForge.csproj
        services.AddTransient<LayoutInteractor<PivotDataSource>>();
        services.AddTransient<LayoutGenerator<PivotDataSource>>();
        services.AddTransient<DataGroupPreparer<PivotDataSource>>();
        services.AddTransient<LegendGenerator>();
        services.AddTransient<ITreemapGenerator, SquarifiedTreemapGenerator>();
    });
```

## License

This project is licensed under the MIT License.
