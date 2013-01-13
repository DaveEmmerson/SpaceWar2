using System;
using System.Linq;
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
    internal class CircleTests
    {
        private readonly Color _color = Color.HotPink;
        
        private IGraphicsDevice _graphicsDevice;
        
        [SetUp]
        public void SetUp()
        {
            _graphicsDevice = Substitute.For<IGraphicsDevice>();
        }

        [Test]
        public void Circle_creates_a_Circle_with_the_correct_number_of_verticies()
        {
            const float radius = 10f;
            const int lineCount = 10;

            var circle = new Circle(radius, _color, lineCount);

            Assert.AreEqual(lineCount, circle.Vertices().Length);
        }

        [Test]
        public void Circle_creates_circle_where_all_verticies_have_correct_radius()
        {
            const float radius = 10f;
            const int lineCount = 10;

            var circle = new Circle(radius, _color, lineCount);

            var radii = from vertex in circle.Vertices()
                        let x = vertex.Position.X
                        let y = vertex.Position.Y
                        select (float) Math.Sqrt((x*x) + (y*y));

            foreach (var actualRadius in radii)
            {
                actualRadius.AssertAreEqualWithinTolerance(radius, 0.000001f);
            }
        }

        [Test]
        public void Circle_creates_circle_where_all_verticies_with_the_correct_colours()
        {
            const float radius = 1f;
            const int lineCount = 8;

            var circle = new Circle(radius, _color, lineCount);

            foreach (var vertex in circle.Vertices())
            {
                Assert.AreEqual(_color, vertex.Color);
            }
        }

        [Test]
        public void Circle_throws_exception_if_called_with_negative_radius()
        {
            const float radius = -1f;
            const int lineCount = 8;

            var exception = Assert.Throws<ArgumentException>(() => new Circle(radius, _color, lineCount));
            Assert.AreEqual("Must be greater than 0\r\nParameter name: radius", exception.Message);
        }

        [Test]
        public void Circle_throws_exception_if_called_with_radius_of_zero()
        {
            const float radius = 0f;
            const int lineCount = 8;

            var exception = Assert.Throws<ArgumentException>(() => new Circle(radius, _color, lineCount));
            Assert.AreEqual("Must be greater than 0\r\nParameter name: radius", exception.Message);
        }

        [Test]
        public void Circle_throws_exception_if_called_with_negative_lineCount()
        {
            const float radius = 1f;
            const int lineCount = -8;

            var exception = Assert.Throws<ArgumentException>(() => new Circle(radius, _color, lineCount));
            Assert.AreEqual("Must be at least 2\r\nParameter name: lineCount", exception.Message);
        }

        [Test]
        public void Circle_throws_exception_if_called_with_lineCount_of_zero()
        {
            const float radius = 1f;
            const int lineCount = 0;

            var exception = Assert.Throws<ArgumentException>(() => new Circle(radius, _color, lineCount));
            Assert.AreEqual("Must be at least 2\r\nParameter name: lineCount", exception.Message);
        }

        [Test]
        public void Draw_calls_DrawUserPrimatives_on_supplied_IGraphicsDevice()
        {
            const float radius = 10f;
            const int lineCount = 10;

            var circle = new Circle(radius, _color, lineCount);

            circle.Draw(_graphicsDevice);

            _graphicsDevice.Received(1).DrawUserPrimitives(PrimitiveType.LineStrip, circle.Vertices(), 0, 9);
        }
    }
}
