using System.Linq;
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

        //TODO we could do some more assertions here, but this would mean exposing some properties
        //on arrow, and I'm not sure that's a good idea, just for these tests.

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

            Assert.IsNotNull(arrow);
            var arrowObject = (Arrow)arrow;

            Assert.AreEqual(Color.Transparent, arrowObject.Verticies[0].Color);
            Assert.AreEqual(color, arrowObject.Verticies[1].Color);
            Assert.AreEqual(color, arrowObject.Verticies[2].Color);
            Assert.AreEqual(color, arrowObject.Verticies[3].Color);
            Assert.AreEqual(color, arrowObject.Verticies[4].Color);
            Assert.AreEqual(color, arrowObject.Verticies[5].Color);
        }

        [Test]
        public void CreateArrow_returns_arrow_that_points_in_the_specified_direction()
        {
            var position = Vector2.Zero;
            var expectedDirection = new Vector2(1f, 5f);
            var color = Color.HotPink;
            const float radius = 13f;

            var arrow = Arrow.CreateArrow(_graphicsDeviceManager, position, expectedDirection, color, radius);

            Assert.IsNotNull(arrow);
            var arrowObject = (Arrow)arrow;

            var x0 = arrowObject.Verticies[0].Position.X;
            var y0 = arrowObject.Verticies[0].Position.Y;
            var x1 = arrowObject.Verticies[1].Position.X;
            var y1 = arrowObject.Verticies[1].Position.Y;

            var actualDirection = new Vector2(x1 - x0, y1 - y0);
            expectedDirection.Normalize();
            actualDirection.Normalize();
            Assert.IsTrue(actualDirection.Matches(expectedDirection));
        }

        [Test]
        public void CreateArrow_returns_arrow_that_starts_in_the_specified_position()
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
            
            Assert.IsTrue(actualPosition.Matches(expectedPosition));
        }

        [Test]
        public void CreateArrow_returns_arrow_with_correct_length()
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
            var expectedPosition = position + (direction * radius);

            Assert.IsTrue(actualPosition.Matches(expectedPosition));
        }
    }

    //todo put all these methods into utility class for getting arrow properties (reverse engineering arrows properites from their verticies
}
