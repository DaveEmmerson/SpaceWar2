using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2.Utils;
using DEMW.SpaceWar2Tests.TestUtils;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class GraphicsFactoryTests
    {
        private GraphicsFactory _graphicsFactory;

        [SetUp]
        public void SetUp()
        {
            _graphicsFactory = new GraphicsFactory();
        }

        [Test]
        public void CreateAccelerationArrow_creates_a_suitable_arrow_with_the_correct_properties()
        {
            var acceleration = new Vector2(-5f, 3f);
            const float radius = 15f;

            var accelerationArrow = _graphicsFactory.CreateAccelerationArrow(acceleration, radius);

            var actualDirection = ArrowUtils.GetDirection(accelerationArrow);
            var expectedDirection = acceleration;
            expectedDirection.Normalize();

            var actualPosition = ArrowUtils.GetPosition(accelerationArrow);
            var expectedPosition = new Vector2(-12.8623943f, 7.71743679f);

            Assert.AreEqual(12.2442112f, ArrowUtils.GetLength(accelerationArrow));
            actualPosition.AssertAreEqualWithinTolerance(expectedPosition);
            actualDirection.AssertAreEqualWithinTolerance(expectedDirection, 0.000001f);
            ArrowUtils.CheckColour(accelerationArrow, Color.LimeGreen);
        }

        [Test]
        public void CreateVelocityArrow_creates_a_suitable_arrow_with_the_correct_properties()
        {
            var velocity = new Vector2(-4f, 0f);
            const float radius = 15f;

            var velocityArrow = _graphicsFactory.CreateVelocityArrow(velocity, radius);

            var actualDirection = ArrowUtils.GetDirection(velocityArrow);
            var expectedDirection = velocity;
            expectedDirection.Normalize();

            var actualPosition = ArrowUtils.GetPosition(velocityArrow);
            var expectedPosition = new Vector2(-15f, 0f);

            Assert.AreEqual(11.0f, ArrowUtils.GetLength(velocityArrow));
            actualPosition.AssertAreEqualWithinTolerance(expectedPosition);
            actualDirection.AssertAreEqualWithinTolerance(expectedDirection);
            ArrowUtils.CheckColour(velocityArrow, Color.Linen);
        }

        [Test]
        public void CreateRotationArrow_creates_a_suitable_arrow_with_the_correct_properties()
        {
            const float rotation = 1.2f;
            const float radius = 15f;

            var rotationArrow = _graphicsFactory.CreateRotationArrow(rotation, radius);
            var expectedDirection = new Vector2(0f, -1f).Rotate(1.2f);
            var actualDirection = ArrowUtils.GetDirection(rotationArrow);

            var actualPosition = ArrowUtils.GetPosition(rotationArrow);
            var expectedPosition = new Vector2(13.9805861f, -5.43536568f);

            Assert.AreEqual(21.4316788f, ArrowUtils.GetLength(rotationArrow), 0.00001f);
            actualPosition.AssertAreEqualWithinTolerance(expectedPosition);
            actualDirection.AssertAreEqualWithinTolerance(expectedDirection, 0.0000001f);
            ArrowUtils.CheckColour(rotationArrow, Color.Red);
        }

        [Test]
        public void CreateForceArrow_creates_a_suitable_arrow_with_the_correct_properties()
        {
            const float radius = 15f;
            var force = new Force(new Vector2(1f, 2f), new Vector2(4, 8f));

            var forceArrow = _graphicsFactory.CreateForceArrow(force, radius);

            var actualDirection = ArrowUtils.GetDirection(forceArrow);
            var expectedDirection = force.Vector;
            expectedDirection.Normalize();

            var actualPosition = ArrowUtils.GetPosition(forceArrow);
            var expectedPosition = new Vector2(10.7082043f, 21.4164085f);

            Assert.AreEqual(9.48604584f, ArrowUtils.GetLength(forceArrow));
            actualPosition.AssertAreEqualWithinTolerance(expectedPosition);
            actualDirection.AssertAreEqualWithinTolerance(expectedDirection);
            ArrowUtils.CheckColour(forceArrow, Color.Yellow);
        }
    }
}
