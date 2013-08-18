using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Utils.XnaWrappers;
using DEMW.SpaceWar2Tests.TestUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using NUnit.Framework;
using System;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class ArrowTests
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
        public void CreateArrow_returns_NullArrow_if_direction_is_zero_vector()
        {
            var position = Vector2.Zero;
            var direction = Vector2.Zero;

            var arrow = Arrow.CreateArrow(position, direction, _color, Radius);

            Assert.IsInstanceOf<NullArrow>(arrow);
        }

        [Test]
        public void CreateArrow_returns_arrow_with_six_verticies_if_direction_is_non_zero_vector()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(position, direction, _color, Radius);

            Assert.IsNotNull(arrow);
            var arrowObject = (Arrow) arrow;

            Assert.AreEqual(6, arrowObject.Vertices().Length);
        }

        [Test]
        public void CreateArrow_creates_all_verticies_with_the_correct_colours()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(position, direction, _color, Radius);

            ArrowUtils.CheckColour(arrow, _color);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_points_in_the_specified_direction()
        {
            var position = Vector2.Zero;
            var expectedDirection = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(position, expectedDirection, _color, Radius);

            var actualDirection = ArrowUtils.GetDirection(arrow);
            expectedDirection.Normalize();
            actualDirection.AssertAreEqualWithinTolerance(expectedDirection, 0.000001f);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_starts_at_specified_radius()
        {
            var position = Vector2.Zero;
            var direction = new Vector2(1f, 5f);

            var arrow = Arrow.CreateArrow(position, direction, _color, Radius);

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

            var arrow = Arrow.CreateArrow(position, direction, _color, Radius);

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

            var arrow = Arrow.CreateArrow(position, direction, _color, Radius);

            var expectedLength = (Radius / 3f) + (3f * (float)Math.Sqrt(direction.Length()));
            var actualLength = ArrowUtils.GetLength(arrow);
            Assert.AreEqual(expectedLength, actualLength);
        }

        [Test]
        public void Draw_calls_DrawUserPrimatives_on_supplied_IGraphicsDevice()
        {
            var position = Vector2.Zero;
            var direction = Vector2.UnitX;
            var arrow = Arrow.CreateArrow(position, direction, _color, Radius);

            arrow.Draw(_graphicsDevice);

            _graphicsDevice.Received(1).DrawUserPrimitives(PrimitiveType.LineStrip, ((Arrow)arrow).Vertices(), 0, 5);
        }
    }
}
