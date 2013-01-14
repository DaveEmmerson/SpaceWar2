using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.GameObjects;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects
{
    [TestFixture]
    internal class ShipComponentFactoryTests
    {
        private ShipComponentFactory _shipComponentFactory;
        private IShip _ship;

        [SetUp]
        public void SetUp()
        {
            _shipComponentFactory = new ShipComponentFactory();
            _ship = Substitute.For<IShip>();
        }

        [Test]
        public void CreateEnergyStore_creates_an_EnergyStore()
        {
            var energyStore = _shipComponentFactory.CreateEnergyStore();

            Assert.AreEqual(100f, energyStore.Level);
        }

        [Test] public void CreateShield_creates_a_Shield()
        {
            var shield = _shipComponentFactory.CreateShield(_ship);

            Assert.AreEqual(100f, shield.Level);
        }

        [Test]
        public void CreateHull_creates_a_Hull()
        {
            var hull = _shipComponentFactory.CreateHull(_ship);

            Assert.AreEqual(100f, hull.Level);
        }

        [Test]
        public void CreateThrusterArray_creates_a_ThrusterArray()
        {
            var thrusterArray = _shipComponentFactory.CreateThrusterArray(_ship);

            Assert.IsNotNull(thrusterArray);
        }
     
    }
}
