using DEMW.SpaceWar2.GameObjects;
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
        
        private IUniverse _universe;
        private readonly Volume _volume = new Volume(-1,1,-2,2,-3,3);
        private DrawingManager _drawingManager;

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
            Assert.IsNotNull(_drawingManager.ActiveCamera);
            var actualVolume = _universe.Received(1).Volume;
            Assert.IsNull(actualVolume);
        }

        [Test]
        public void ResetCamera_creates_a_new_camera_with_the_provided_universe()
        {
            var originalCamera = _drawingManager.ActiveCamera;
            
            _drawingManager.ResetCamera(_universe);

            Assert.AreNotSame(originalCamera, _drawingManager.ActiveCamera);
            var actualVolume = _universe.Received(2).Volume;
            Assert.IsNull(actualVolume);
        }

        [Test]
        public void ResetCamera_does_not_replace_camera_when_the_provided_universe_is_null()
        {
            var originalCamera = _drawingManager.ActiveCamera;

            _drawingManager.ResetCamera(null);

            Assert.AreSame(originalCamera, _drawingManager.ActiveCamera);
            var actualVolume = _universe.Received(1).Volume;
            Assert.IsNull(actualVolume);
        }

        [Test]
        public void Register_adds_a_GameObject_to_the_DrawingManager()
        {
            var gameObject = Substitute.For<IGameObject>();

            _drawingManager.Register(gameObject);
            Assert.AreEqual(1, _drawingManager.ObjectCount);
        }

        [Test]
        public void Unregister_removes_a_GameObject_to_the_DrawingManager()
        {
            var gameObject = Substitute.For<IGameObject>();

            _drawingManager.Register(gameObject);
            _drawingManager.Unregister(gameObject);

            Assert.AreEqual(0, _drawingManager.ObjectCount);
        }
    }
}
