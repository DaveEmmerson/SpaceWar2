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
        private readonly Color _color = Color.HotPink;
        private const float Radius = 13f;

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

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, _color, Radius);

            Assert.IsNull(arrow);
        }

        [Test]
        public void CreateArrow_returns_arrow_with_six_verticies_if_direction_is_non_zero_vector()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, _color, Radius);

            Assert.IsNotNull(arrow);
            var arrowObject = (Arrow) arrow;

            Assert.AreEqual(6, arrowObject.Verticies.Length);
        }

        [Test]
        public void CreateArrow_creates_all_verticies_with_the_correct_colours()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, _color, Radius);

            ArrowUtils.CheckColour(arrow, _color);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_points_in_the_specified_direction()
        {
            var position = Vector2.Zero;
            var expectedDirection = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, expectedDirection, _color, Radius);

            var actualDirection = ArrowUtils.GetDirection(arrow);
            expectedDirection.Normalize();
            actualDirection.AssertAreEqualWithinTolerance(expectedDirection, 0.000001f);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_starts_at_specified_radius()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, _color, Radius);

            var actualPosition = ArrowUtils.GetPosition(arrow);
            direction.Normalize();
            var expectedPosition = position + (direction*Radius);

            actualPosition.AssertAreEqualWithinTolerance(expectedPosition);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_starts_in_the_specified_position()
        {
            var position = new Vector2(3f, 7f);
            var direction = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, _color, Radius);

            var actualPosition = ArrowUtils.GetPosition(arrow);
            direction.Normalize();
            var expectedPosition = position + (direction * Radius);

            actualPosition.AssertAreEqualWithinTolerance(expectedPosition);
        }

        [Test]
        public void CreateArrow_returns_arrow_with_correct_length()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, direction, _color, Radius);

            var expectedLength = (Radius / 3f) + (3f * (float)Math.Sqrt(direction.Length()));
            var actualLength = ArrowUtils.GetLength(arrow);
            Assert.AreEqual(expectedLength, actualLength);
        }
    }
}
