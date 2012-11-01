using DEMW.SpaceWar2.Physics;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class UniverseTests
    {
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Universe_creates_universe_and_initialises_the_dimension_properties()
        {
            var universe = new Universe(-600f, 600f, -400f, 400f, -100f, 100f);

            Assert.AreEqual(-600, universe.MinX);
            Assert.AreEqual(600, universe.MaxX);
            Assert.AreEqual(-400, universe.MinY);
            Assert.AreEqual(400, universe.MaxY);
            Assert.AreEqual(-100, universe.MinZ);
            Assert.AreEqual(100, universe.MaxZ);

            Assert.AreEqual(1200f, universe.Width);
            Assert.AreEqual(800f, universe.Height);
        }

        //TODO Add test for the rest of Universe
    }
}
