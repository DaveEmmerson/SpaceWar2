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
    internal class NullArrowTests
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
        public void NullArrow_can_be_created_but_does_nothing()
        {
            var arrow = new NullArrow();
            Assert.DoesNotThrow(() => arrow.Draw(null));
            Assert.IsEmpty(arrow.Verticies);
        }
    }
}
