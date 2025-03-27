using SquarifiedTreemapForge;
using SquarifiedTreemapForge.Blazor.Components;
using SquarifiedTreemapForge.Layout;
using SquarifiedTreemapForge.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddRazorComponents()
        .AddInteractiveServerComponents();

    // Configure settings
    services.Configure<TreemapSettings>(
        configuration.GetSection("TreemapSettings"));
    services.Configure<TreemapLayoutSettings>(
        configuration.GetSection("TreemapLayoutSettings"));
    services.Configure<LegendSettings>(
        configuration.GetSection("LegendSettings"));

    // Register services
    services.AddTransient<TreemapSettings>();
    services.AddTransient<TreemapLayoutSettings>();
    services.AddTransient<LegendSettings>();
    services.AddTransient<LayoutInteractor<PivotDataSource>>();
    services.AddTransient<LayoutGenerator<PivotDataSource>>();
    services.AddTransient<DataGroupPreparer<PivotDataSource>>();
    services.AddTransient<LegendGenerator>();
    services.AddTransient<ITreemapGenerator, SquarifiedTreemapGenerator>();
}