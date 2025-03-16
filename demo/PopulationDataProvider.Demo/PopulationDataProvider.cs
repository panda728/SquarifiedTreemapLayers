using System.Text.Json;

namespace PopulationDataProvider.Demo;

public class PopulationDataProvider
{
    public IReadOnlyList<PopulationData> GetPopulationPivotsFromJson()
    {
        var json = File.ReadAllText("Population.json");
        return JsonSerializer.Deserialize<List<PopulationData>>(json) ?? [];
    }
}