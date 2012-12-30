using System;
using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Utils.XnaWrappers;
using DEMW.SpaceWar2Tests.TestUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class CameraTests
    {
        private readonly Color _color = Color.HotPink;
        private const float Radius = 13f;

        private IGraphicsDevice _graphicsDevice;
        
        [SetUp]
        public void SetUp()
        {
            _graphicsDevice = Substitute.For<IGraphicsDevice>();
        }

        [Test]
        public void Camera_returns_camera_with_default_target_and_position_projecting_all_of_volume()
        {
            var position = Vector2.Zero;
            var target = Vector2.Zero;

            throw new NotImplementedException();
            //get and test view
            //get and test projection
        }

        [Test]
        public void Pan_updates_View_by_the_specified_Vector()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Zoom_changes_Volume_that_is_visible_causing_the_projection_to_change_accordingly()
        {
            throw new NotImplementedException();
        }
    }
}
