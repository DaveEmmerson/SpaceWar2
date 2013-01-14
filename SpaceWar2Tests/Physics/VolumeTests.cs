using DEMW.SpaceWar2.Core.Physics;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class VolumeTests
    {
        [Test]
        public void Volume_creates_volume_and_initialises_the_dimension_properties()
        {
            var volume = new Volume(-600f, 600f, -400f, 400f, -100f, 100f);

            Assert.AreEqual(-600, volume.MinX);
            Assert.AreEqual(600, volume.MaxX);
            Assert.AreEqual(-400, volume.MinY);
            Assert.AreEqual(400, volume.MaxY);
            Assert.AreEqual(-100, volume.MinZ);
            Assert.AreEqual(100, volume.MaxZ);

            Assert.AreEqual(1200f, volume.Width);
            Assert.AreEqual(800f, volume.Height);
        }

        [TestCase(100)]
        [TestCase(-50)]
        [TestCase(0)]
        public void Expand_increases_size_of_Volume_vertically_by_amount_and_maintains_horizontal_proportion(float verticalAmount)
        {
            var volume = new Volume(-400, 400, -240, 240, -1000, 1000);
            var proportion = volume.Width / volume.Height;

            volume.Expand(verticalAmount);

            Assert.AreEqual(-400f - (verticalAmount * proportion), volume.MinX);
            Assert.AreEqual(400f + (verticalAmount * proportion), volume.MaxX);
            Assert.AreEqual(-240f - verticalAmount, volume.MinY);
            Assert.AreEqual(240f + verticalAmount, volume.MaxY);
            Assert.AreEqual(-1000f, volume.MinZ);
            Assert.AreEqual(1000f, volume.MaxZ);
            Assert.AreEqual(800 + (verticalAmount*2*proportion), volume.Width);
            Assert.AreEqual(480 + (verticalAmount*2), volume.Height);
        }

        [TestCase(100)]
        [TestCase(-50)]
        [TestCase(0)]
        public void Contract_decreases_size_of_Volume_vertically_by_amount_and_maintains_horizontal_proportion(float verticalAmount)
        {
            var volume = new Volume(-400, 400, -240, 240, -1000, 1000);
            var horizontalAmount = verticalAmount * volume.Width / volume.Height;

            volume.Contract(verticalAmount);

            Assert.AreEqual(-400f + horizontalAmount, volume.MinX);
            Assert.AreEqual(400f - horizontalAmount, volume.MaxX);
            Assert.AreEqual(-240f + verticalAmount, volume.MinY);
            Assert.AreEqual(240f - verticalAmount, volume.MaxY);
            Assert.AreEqual(-1000f, volume.MinZ);
            Assert.AreEqual(1000f, volume.MaxZ);

            Assert.AreEqual(800 - (horizontalAmount * 2), volume.Width);
            Assert.AreEqual(480 - (verticalAmount * 2), volume.Height);
        }

        [Test]
        public void Clone_returns_a_copy_of_Volume_with_same_dimensions()
        {
            var volume = new Volume(-400, 400, -240, 240, -1000, 1000);
            
            var clone = volume.Clone();

            Assert.AreNotSame(volume, clone);
            Assert.AreEqual(volume.MinX, clone.MinX);
            Assert.AreEqual(volume.MaxX, clone.MaxX);
            Assert.AreEqual(volume.MinY, clone.MinY);
            Assert.AreEqual(volume.MaxY, clone.MaxY);
            Assert.AreEqual(volume.MinZ, clone.MinZ);
            Assert.AreEqual(volume.MaxZ, clone.MaxZ);
        }
    }
}
