using System.Drawing;

namespace SquarifiedTreemapForge.Shared;

public interface ITreemapGenerator
{
    IEnumerable<Rectangle> Layout(Rectangle bounds, IEnumerable<double> weights, LayoutAlign layout = LayoutAlign.LeftTop, int minimumSize = 2, bool isCheckSorted = true);
}
