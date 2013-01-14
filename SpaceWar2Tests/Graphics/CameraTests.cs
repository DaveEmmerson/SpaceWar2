using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Physics;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class CameraTests
    {
        private Volume _volume;

        [SetUp]
        public void SetUp()
        {
            _volume = new Volume(-100f, 100f, -200f, 200f, -10f, 10f);
        }

        [Test]
        public void Camera_returns_camera_with_default_view_and_projection_to_cover_all_of_volume()
        {
            var camera = new Camera(_volume);

            var actualView = camera.View;
            var actualProjection = camera.Projection;

            var expectedView = new Matrix(1f, 0f, 0f, 0f,
                                          0f, 1f, 0f, 0f, 
                                          0f, 0f, 1f, 0f,
                                          0f, 0f, -1f, 1f);

            var expectedProjection = new Matrix(0.01f, 0f, 0f, 0f, 
                                                0f, -0.005f, 0f, 0f, 
                                                0f, 0f, -0.05f, 0f, 
                                                0f, 0f, 0.5f, 1f);

            Assert.AreEqual(expectedView, actualView);
            Assert.AreEqual(expectedProjection, actualProjection);
        }

        [Test]
        public void Pan_updates_View_by_the_specified_Vector_and_does_not_affect_Projection()
        {
            var camera = new Camera(_volume);

            camera.Pan(new Vector3(1f,2f,4f));

            var actualView = camera.View;
            var actualProjection = camera.Projection;

            var expectedView = new Matrix(1f, 0f, 0f, 0f,
                                          0f, 1f, 0f, 0f,
                                          0f, 0f, 1f, 0f,
                                          -1f, -2f, -5f, 1f);

            var expectedProjection = new Matrix(0.01f, 0f, 0f, 0f,
                                                0f, -0.005f, 0f, 0f,
                                                0f, 0f, -0.05f, 0f,
                                                0f, 0f, 0.5f, 1f);

            Assert.AreEqual(expectedView, actualView);
            Assert.AreEqual(expectedProjection, actualProjection);
        }

        [Test]
        public void Zoom_changes_Volume_that_is_visible_causing_the_Projection_to_change_accordingly()
        {
            var camera = new Camera(_volume);

            camera.Zoom(10f);

            var actualView = camera.View;
            var actualProjection = camera.Projection;

            var expectedView = new Matrix(1f, 0f, 0f, 0f,
                                          0f, 1f, 0f, 0f,
                                          0f, 0f, 1f, 0f,
                                          0f, 0f, -1f, 1f);

            var expectedProjection = new Matrix(0.00952381f, 0f, 0f, 0f,
                                                0f, -0.004761905f, 0f, 0f,
                                                0f, 0f, -0.05f, 0f,
                                                0f, 0f, 0.5f, 1f);

            Assert.AreEqual(expectedView, actualView);
            Assert.AreEqual(expectedProjection, actualProjection);
        }
    }
}
