using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class UniverseTests
    {
        [Test]
        public void Universe_creates_universe_and_initialises_the_dimension_properties()
        {
            var universe = new Universe(-600f, 600f, -400f, 400f, -100f, 100f);

            Assert.AreEqual(-600, universe.MinX);
            Assert.AreEqual(600, universe.MaxX);
            Assert.AreEqual(-400, universe.MinY);
            Assert.AreEqual(400, universe.MaxY);
            Assert.AreEqual(-100, universe.MinZ);
            Assert.AreEqual(100, universe.MaxZ);

            Assert.AreEqual(1200f, universe.Width);
            Assert.AreEqual(800f, universe.Height);
        }

        [Test]
        public void CopyDimensions_creates_new_Universe_with_the_same_dimensions()
        {
            var universe = new Universe(-600f, 600f, -400f, 400f, -100f, 100f);
            var copy = universe.CopyDimensions();

            Assert.AreNotSame(universe, copy);
            Assert.AreEqual(universe.MinX, copy.MinX);
            Assert.AreEqual(universe.MaxX, copy.MaxX);
            Assert.AreEqual(universe.MinY, copy.MinY);
            Assert.AreEqual(universe.MaxY, copy.MaxY);
            Assert.AreEqual(universe.MinZ, copy.MinZ);
            Assert.AreEqual(universe.MaxZ, copy.MaxZ);
        }

        [Test]
        public void GetDefault_creates_a_new_Universe_with_default_dimensions()
        {
            var defaultUniverse = Universe.GetDefault();
            
            Assert.AreEqual(-400f, defaultUniverse.MinX);
            Assert.AreEqual(400f, defaultUniverse.MaxX);
            Assert.AreEqual(-240f, defaultUniverse.MinY);
            Assert.AreEqual(240f, defaultUniverse.MaxY);
            Assert.AreEqual(-1000f, defaultUniverse.MinZ);
            Assert.AreEqual(1000f, defaultUniverse.MaxZ);
        }

        [TestCase(100)]
        [TestCase(-50)]
        [TestCase(0)]
        public void Expand_increases_size_of_Universe_vertically_by_amount_and_maintains_horizontal_proportion(float verticalAmount)
        {
            var defaultUniverse = Universe.GetDefault();
            var proportion = defaultUniverse.Width / defaultUniverse.Height;

            defaultUniverse.Expand(verticalAmount);
       
            Assert.AreEqual(-400f - (verticalAmount * proportion), defaultUniverse.MinX);
            Assert.AreEqual(400f + (verticalAmount * proportion), defaultUniverse.MaxX);
            Assert.AreEqual(-240f - verticalAmount, defaultUniverse.MinY);
            Assert.AreEqual(240f + verticalAmount, defaultUniverse.MaxY);
            Assert.AreEqual(-1000f, defaultUniverse.MinZ);
            Assert.AreEqual(1000f, defaultUniverse.MaxZ);
        }

        [TestCase(100)]
        [TestCase(-50)]
        [TestCase(0)]
        public void Contract_decreases_size_of_Universe_vertically_by_amount_and_maintains_horizontal_proportion(float verticalAmount)
        {
            var defaultUniverse = Universe.GetDefault();
            var horizontalAmount = verticalAmount * defaultUniverse.Width / defaultUniverse.Height;

            defaultUniverse.Contract(verticalAmount);

            Assert.AreEqual(-400f + horizontalAmount, defaultUniverse.MinX);
            Assert.AreEqual(400f - horizontalAmount, defaultUniverse.MaxX);
            Assert.AreEqual(-240f + verticalAmount, defaultUniverse.MinY);
            Assert.AreEqual(240f - verticalAmount, defaultUniverse.MaxY);
            Assert.AreEqual(-1000f, defaultUniverse.MinZ);
            Assert.AreEqual(1000f, defaultUniverse.MaxZ);
        }

        public void Update_constrains_registered_objects_to_the_dimensions_of_the_universe()
        {
            var universe = new Universe(-600f, 600f, -400f, 400f, -100f, 100f);
            var gameObject1 = Substitute.For<IGameObject>();
            var gameObject2 = Substitute.For<IGameObject>();
            var gameObject3 = Substitute.For<IGameObject>();
            var gameObject4 = Substitute.For<IGameObject>();
            gameObject1.Position.Returns(new Vector2(universe.MinX - 10f, 0f));
            gameObject2.Position.Returns(new Vector2(universe.MaxX - 10f, universe.MaxY + 10f));
            gameObject3.Position.Returns(new Vector2(universe.MaxX + 10f, universe.MaxY - 10f));
            gameObject4.Position.Returns(new Vector2(universe.MinX - universe.Width - 20f, universe.MinY + universe.Height / 2));

            universe.Register(gameObject1);
            universe.Register(gameObject2);
            universe.Register(gameObject3);
            universe.Register(gameObject4);

            universe.Update();

            var expectedPosition1 = new Vector2(universe.MaxX - 10f, 0f);
            gameObject1.Received().Teleport(Arg.Is(expectedPosition1));

            var expectedPosition2 = new Vector2(universe.MaxX - 10f, universe.MinY + 10f);
            gameObject2.Received().Teleport(Arg.Is(expectedPosition2));

            var expectedPosition3 = new Vector2(universe.MinX + 10f, universe.MaxY - 10f);
            gameObject3.Received().Teleport(Arg.Is(expectedPosition3));

            var expectedPosition4 = new Vector2(universe.MaxX - 20f, universe.MinY + universe.Height / 2);
            gameObject4.Received().Teleport(Arg.Is(expectedPosition4));

        }

        [Test]
        public void UnRegister_removes_registered_object()
        {
            var universe = new Universe(-600f, 600f, -400f, 400f, -100f, 100f);
            var gameObject1 = Substitute.For<IGameObject>();
            gameObject1.Position.Returns(new Vector2(universe.MinX - 10f, 0f));

            universe.Register(gameObject1);
            universe.UnRegister(gameObject1);

            universe.Update();

            gameObject1.DidNotReceive().Teleport(Arg.Any<Vector2>());
        }
    }
}
