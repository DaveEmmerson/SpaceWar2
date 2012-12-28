using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2.Utils;
using DEMW.SpaceWar2Tests.TestUtils;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class GraphicsFactoryTests
    {
        private IGraphicsDeviceManager _graphicsDeviceManager;
        private GraphicsFactory _graphicsFactory;

        [SetUp]
        public void SetUp()
        {
            _graphicsDeviceManager = Substitute.For<IGraphicsDeviceManager>();
            _graphicsFactory = new GraphicsFactory(_graphicsDeviceManager);
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
            
            Assert.AreEqual(12.2442112f, ArrowUtils.GetLength(accelerationArrow));
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

            Assert.AreEqual(11.0f, ArrowUtils.GetLength(velocityArrow));
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

            Assert.AreEqual(21.4316788f, ArrowUtils.GetLength(rotationArrow), 0.00001f);
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

            Assert.AreEqual(9.48604584f, ArrowUtils.GetLength(forceArrow));
            actualDirection.AssertAreEqualWithinTolerance(expectedDirection);
            ArrowUtils.CheckColour(forceArrow, Color.Yellow);
        }
    }
}
