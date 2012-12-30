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
        public void Universe_creates_universe_and_initialises_the_volume_property()
        {
            var volume = new Volume(-600f, 600f, -400f, 400f, -100f, 100f);
            var universe = new Universe(volume);

            Assert.AreEqual(volume, universe.Volume);
        }

        [Test]
        public void GetDefault_creates_a_new_Universe_with_default_dimensions()
        {
            var defaultUniverse = Universe.GetDefault();
            
            Assert.AreEqual(-400f, defaultUniverse.Volume.MinX);
            Assert.AreEqual(400f, defaultUniverse.Volume.MaxX);
            Assert.AreEqual(-240f, defaultUniverse.Volume.MinY);
            Assert.AreEqual(240f, defaultUniverse.Volume.MaxY);
            Assert.AreEqual(-1000f, defaultUniverse.Volume.MinZ);
            Assert.AreEqual(1000f, defaultUniverse.Volume.MaxZ);
        }

        [Test]
        public void Update_constrains_registered_objects_to_the_dimensions_of_the_universe()
        {
            var volume = new Volume(-600f, 600f, -400f, 400f, -100f, 100f);
            var universe = new Universe(volume);
            var gameObject1 = Substitute.For<IGameObject>();
            var gameObject2 = Substitute.For<IGameObject>();
            var gameObject3 = Substitute.For<IGameObject>();
            var gameObject4 = Substitute.For<IGameObject>();
            gameObject1.Position.Returns(new Vector2(volume.MinX - 10f, 0f));
            gameObject2.Position.Returns(new Vector2(volume.MaxX - 10f, volume.MaxY + 10f));
            gameObject3.Position.Returns(new Vector2(volume.MaxX + 10f, volume.MaxY - 10f));
            gameObject4.Position.Returns(new Vector2(volume.MinX - volume.Width - 20f, volume.MinY + volume.Height / 2));

            universe.Register(gameObject1);
            universe.Register(gameObject2);
            universe.Register(gameObject3);
            universe.Register(gameObject4);

            universe.Update();

            var expectedPosition1 = new Vector2(volume.MaxX - 10f, 0f);
            gameObject1.Received().Teleport(Arg.Is(expectedPosition1));

            var expectedPosition2 = new Vector2(volume.MaxX - 10f, volume.MinY + 10f);
            gameObject2.Received().Teleport(Arg.Is(expectedPosition2));

            var expectedPosition3 = new Vector2(volume.MinX + 10f, volume.MaxY - 10f);
            gameObject3.Received().Teleport(Arg.Is(expectedPosition3));

            var expectedPosition4 = new Vector2(volume.MaxX - 20f, volume.MinY + volume.Height / 2);
            gameObject4.Received().Teleport(Arg.Is(expectedPosition4));

        }

        [Test]
        public void UnRegister_removes_registered_object()
        {
            var volume = new Volume(-600f, 600f, -400f, 400f, -100f, 100f);
            var universe = new Universe(volume);
            var gameObject1 = Substitute.For<IGameObject>();
            gameObject1.Position.Returns(new Vector2(volume.MinX - 10f, 0f));

            universe.Register(gameObject1);
            universe.UnRegister(gameObject1);

            universe.Update();

            gameObject1.DidNotReceive().Teleport(Arg.Any<Vector2>());
        }
    }
}
