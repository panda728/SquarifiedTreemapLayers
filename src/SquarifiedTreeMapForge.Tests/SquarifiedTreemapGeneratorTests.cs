using System.Drawing;
using NUnit.Framework;

namespace SquarifiedTreemapForge.Tests
{
    [TestFixture]
    public class SquarifiedTreemapGeneratorTests
    {
        [Test]
        public void GenerateTreemap_EmptyWeights_ReturnsEmpty()
        {
            var gen = new SquarifiedTreemapGenerator();
            var result = gen.Layout(new Rectangle(0, 0, 100, 100), []);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GenerateTreemap_BoundsTooSmall_ReturnsEmpty()
        {
            var weights = new List<double> { 1.0, 2.0, 3.0 };
            var gen = new SquarifiedTreemapGenerator();
            var result = gen.Layout(new Rectangle(0, 0, 1, 1), weights);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GenerateTreemap_UnsortedWeights_ThrowsArgumentException()
        {
            var weights = new List<double> { 3.0, 1.0, 2.0 };
            var gen = new SquarifiedTreemapGenerator();
            Assert.Throws<ArgumentException>(() => gen.Layout(new Rectangle(0, 0, 100, 100), weights).ToList());
        }

        [Test]
        public void GenerateTreemap_ValidWeights_ReturnsRectangles()
        {
            var weights = new List<double> { 3.0, 2.0, 1.0 };
            var gen = new SquarifiedTreemapGenerator();
            var result = gen.Layout(new Rectangle(0, 0, 100, 100), weights).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.All(r => r.Width > 0 && r.Height > 0), Is.True);
        }

        [Test]
        public void GenerateTreemap_ValidWeights_RectanglesFitWithinBounds()
        {
            var weights = new List<double> { 3.0, 2.0, 1.0 };
            var gen = new SquarifiedTreemapGenerator();
            var result = gen.Layout(new Rectangle(0, 0, 100, 100), weights).ToList();

            foreach (var rect in result)
            {
                Assert.That(rect.X >= 0 && rect.Y >= 0, Is.True);
                Assert.That(rect.Right <= 100 && rect.Bottom <= 100, Is.True);
            }
        }
    }
}
