using System;
using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2Tests.TestUtils;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class ArrowTests
    {
        private IGraphicsDeviceManager _graphicsDeviceManager;

        [SetUp]
        public void SetUp()
        {
            _graphicsDeviceManager = Substitute.For<IGraphicsDeviceManager>();
        }

        [Test]
        public void CreateArrow_returns_null_if_direction_is_zero_vector()
        {
            var position = Vector2.Zero;
            var direction = Vector2.Zero;
            var color = Color.HotPink;
            const float radius = 13f;

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, color, radius);

            Assert.IsNull(arrow);
        }

        [Test]
        public void CreateArrow_returns_arrow_with_six_verticies_if_direction_is_non_zero_vector()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);
            var color = Color.HotPink;
            const float radius = 13f;

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, color, radius);

            Assert.IsNotNull(arrow);
            var arrowObject = (Arrow) arrow;

            Assert.AreEqual(6, arrowObject.Verticies.Length);
        }

        [Test]
        public void CreateArrow_creates_all_verticies_with_the_correct_colours()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);
            var color = Color.HotPink;
            const float radius = 13f;

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, color, radius);

            ArrowUtils.CheckColour(arrow, color);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_points_in_the_specified_direction()
        {
            var position = Vector2.Zero;
            var expectedDirection = new Vector2(1f, 5f);
            var color = Color.HotPink;
            const float radius = 13f;

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, expectedDirection, color, radius);

            var actualDirection = ArrowUtils.GetDirection(arrow);

            expectedDirection.Normalize();
            actualDirection.AssertAreEqualWithinTolerance(expectedDirection, 0.000001f);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_starts_at_specified_radius()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);
            var color = Color.HotPink;
            const float radius = 13f;

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, color, radius);

            Assert.IsNotNull(arrow);
            var arrowObject = (Arrow)arrow;

            var x0 = arrowObject.Verticies[0].Position.X;
            var y0 = arrowObject.Verticies[0].Position.Y;
            var actualPosition = new Vector2(x0, y0);

            direction.Normalize();
            var expectedPosition = position + (direction*radius);

            actualPosition.AssertAreEqualWithinTolerance(expectedPosition);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_starts_in_the_specified_position()
        {
            var position = new Vector2(3f, 7f);
            var direction = new Vector2(1f, 5f);
            var color = Color.HotPink;
            const float radius = 13f;

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, color, radius);

            Assert.IsNotNull(arrow);
            var arrowObject = (Arrow)arrow;

            var x0 = arrowObject.Verticies[0].Position.X;
            var y0 = arrowObject.Verticies[0].Position.Y;
            var actualPosition = new Vector2(x0, y0);

            direction.Normalize();
            var expectedPosition = position + (direction * radius);

            actualPosition.AssertAreEqualWithinTolerance(expectedPosition);
        }

        [Test]
        public void CreateArrow_returns_arrow_with_correct_length()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);
            var color = Color.HotPink;
            const float radius = 13f;

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, color, radius);
            var actualLength = ArrowUtils.GetLength(arrow);

            var expectedLength = (radius / 3f) + (3f * (float)Math.Sqrt(direction.Length()));

            Assert.AreEqual(expectedLength, actualLength);
        }
    }
}
