using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapForge.Layout;

public sealed class Seed<T>(
    int id,
    int depth,
    string text,
    double weight,
    IEnumerable<T> sources,
    NodeFormat nodeFormat,
    Seed<T>? parent = null,
    List<Seed<T>>? children = null
)
{
    public int Id { get; set; } = id;
    public int Depth { get; set; } = depth;
    public string Text { get; set; } = text;
    public double Weight { get; set; } = weight;
    public T[] Sources { get; set; } = [.. sources ?? []];
    public NodeFormat Format = nodeFormat;
    public Seed<T>? Parent { get; set; } = parent;
    public List<Seed<T>> Children { get; set; } = children ?? [];

    public IEnumerable<T> GetSources(int id) => Seed<T>.GetSources(id, children);

    static IEnumerable<T> GetSources(int id, IEnumerable<Seed<T>>? seeds)
    {
        if (seeds == null || !seeds.Any())
        {
            return [];
        }
        foreach (var s in seeds)
        {
            if (s.Id == id)
            {
                return s.Sources?.AsEnumerable() ?? [];
            }
            if (s.Children == null || s.Children.Count == 0)
            {
                continue;
            }
            var result = GetSources(id, s.Children);
            if (result != null && result.Any()) return result;
        }
        return [];
    }
}
