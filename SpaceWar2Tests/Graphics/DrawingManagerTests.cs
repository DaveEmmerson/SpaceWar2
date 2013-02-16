using System;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Physics;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class DrawingManagerTests
    {
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

        [Test]
        public void MoveCamera_moves_the_camera_by_the_specified_vector()
        {
            var translation = new Vector3(1, 2, 4);
            var beforeProjection = _drawingManager.ActiveCamera.Projection;
            var beforeView = _drawingManager.ActiveCamera.View;
            
            _drawingManager.MoveCamera(translation);

            var afterProjection = _drawingManager.ActiveCamera.Projection;
            var afterView = _drawingManager.ActiveCamera.View;

            Assert.AreEqual(beforeProjection, afterProjection);
            Assert.AreNotEqual(beforeView, afterView);


            var expectedDiff = new Matrix(0f, 0f, 0f, 0f,
                                          0f, 0f, 0f, 0f,
                                          0f, 0f, 0f, 0f,
                                          translation.X, translation.Y, translation.Z, 0f);

            var actualDiff = beforeView - afterView;
            Assert.AreEqual(expectedDiff, actualDiff);
        }

        [Test]
        public void ZoomCamera_zooms_the_camera_by_the_specified_amount()
        {
            var beforeView = _drawingManager.CameraView;
            var beforeProjection = _drawingManager.CameraProjection;

            _drawingManager.ZoomCamera(10f);

            var afterView = _drawingManager.CameraView;
            var afterProjection = _drawingManager.CameraProjection;

            Assert.AreNotEqual(beforeProjection, afterProjection);
            Assert.AreEqual(beforeView, afterView);

            var expectedView = new Matrix(1f, 0f, 0f, 0f,
                                          0f, 1f, 0f, 0f,
                                          0f, 0f, 1f, 0f,
                                          0f, 0f, -1f, 1f);

            var expectedProjection = new Matrix(0.166666672f, 0f, 0f, 0f,
                                                0f, -0.0833333358f, 0f, 0f,
                                                0f, 0f, -0.166666672f, 0f,
                                                0f, 0f, 0.5f, 1f);

            Assert.AreEqual(expectedView, afterView);
            Assert.AreEqual(expectedProjection, afterProjection);
            Assert.AreEqual(expectedProjection.M22, afterProjection.M22);
            Assert.AreEqual(expectedProjection.M33, afterProjection.M33);
            Assert.AreEqual(expectedProjection.M43, afterProjection.M43);
            Assert.AreEqual(expectedProjection.M44, afterProjection.M44);
        }

        [Test]
        public void CameraView_returns_the_ActiveCameras_View()
        {
            var cameraView = _drawingManager.CameraView;
            Assert.AreEqual(_drawingManager.ActiveCamera.View, cameraView);
        }

        [Test]
        public void CameraProjection_returns_the_ActiveCameras_Projection()
        {
            var cameraProjection = _drawingManager.CameraProjection;
            Assert.AreEqual(_drawingManager.ActiveCamera.Projection, cameraProjection);
        }

        //TODO Figure out how we can Test Draw (given the difficulty in mocking XNA)
    }
}
