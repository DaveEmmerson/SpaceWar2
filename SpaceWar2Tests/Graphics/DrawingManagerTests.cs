using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Physics;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class DrawingManagerTests
    {
        //Test Add
        //Test Draw
        //Test Remove
        //Test ResetUniverse
        //Test GetCamera

        private IUniverse _universe;
        private DrawingManager _drawingManager;
        private readonly Volume _volume = new Volume(-1,1,-2,2,-3,3);

        [SetUp]
        public void SetUp()
        {
            _universe = Substitute.For<IUniverse>();
            _universe.Volume.Returns(_volume);

            _drawingManager = new DrawingManager(_universe);
        }

        [Test]
        public void DrawingManager_creates_DrawingManager_and_sets_up_the_Camera_using_the_provided_IUniverse()
        {
            var actualVolume = _universe.Received(1).Volume;
            Assert.IsNull(actualVolume);
            
            Assert.IsNotNull(_drawingManager.ActiveCamera);
        }
    }
}
