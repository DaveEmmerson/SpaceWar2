using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects
{
    [TestFixture]
    internal class ShipTests
    {
        private IGraphicsDeviceManager _graphics;
        private IShipComponentFactory _shipComponentFactory;
        private IEnergyStore _energyStore;
        private IShield _shield;
        private IHull _hull;
        private IThrusterArray _thrusterArray;

        [SetUp]
        public void SetUp()
        {
            _graphics = Substitute.For<IGraphicsDeviceManager>();
            _shipComponentFactory = Substitute.For<IShipComponentFactory>();

            _energyStore = Substitute.For<IEnergyStore>();
            _shield = Substitute.For<IShield>();
            _hull = Substitute.For<IHull>();
            _thrusterArray = Substitute.For<IThrusterArray>();

            _shipComponentFactory.CreateEnergyStore().ReturnsForAnyArgs(_energyStore);
            _shipComponentFactory.CreateSheild(Arg.Any<IShip>()).Returns(_shield);
            _shipComponentFactory.CreateHull(Arg.Any<IShip>()).Returns(_hull);
            _shipComponentFactory.CreateThrusterArray(Arg.Any<IShip>()).Returns(_thrusterArray);
        }

        [Test]
        public void Ship_can_be_created_and_its_properties_are_set_in_the_constructor()
        {
            var position = new Vector2(12f, 5.5f);
            var ship = new Ship("TestShip", position, 16f, Color.Goldenrod, _graphics, _shipComponentFactory);

            Assert.AreEqual("TestShip", ship.Name);
            Assert.AreEqual(Color.Goldenrod, ship.Color);

            Assert.AreEqual(position, ship.Position);
            Assert.AreEqual(16f, ship.Radius);
            Assert.AreEqual(1f, ship.Mass);

            _shipComponentFactory.Received(1).CreateEnergyStore();
            _shipComponentFactory.Received(1).CreateSheild(ship);
            _shipComponentFactory.Received(1).CreateHull(ship);
            _shipComponentFactory.Received(1).CreateThrusterArray(ship);
        }
    }
}
