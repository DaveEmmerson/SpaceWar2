using DEMW.SpaceWar2;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests
{
    [TestFixture]
    class SpaceWar2GameTests
    {
        // This test only exists so that the main SpaceWar2 assembly is included in the coverage report. 
        // If there's a better way to make that happen then this test can be removed.
        [Test]
        public void DummyTest()
        {
            SpaceWar2Game.DummyTestMethod();
        }
    }
}
