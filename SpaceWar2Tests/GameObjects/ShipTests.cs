using System;
using DEMW.SpaceWar2.Controls;
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
        private IShipController _controller;

        private Vector2 _position;
        private Ship _ship;

        [SetUp]
        public void SetUp()
        {
            _graphics = Substitute.For<IGraphicsDeviceManager>();
            _shipComponentFactory = Substitute.For<IShipComponentFactory>();

            _energyStore = Substitute.For<IEnergyStore>();
            _shield = Substitute.For<IShield>();
            _hull = Substitute.For<IHull>();
            _thrusterArray = Substitute.For<IThrusterArray>();
            _controller = Substitute.For<IShipController>();

            _shipComponentFactory.CreateEnergyStore().ReturnsForAnyArgs(_energyStore);
            _shipComponentFactory.CreateSheild(Arg.Any<IShip>()).Returns(_shield);
            _shipComponentFactory.CreateHull(Arg.Any<IShip>()).Returns(_hull);
            _shipComponentFactory.CreateThrusterArray(Arg.Any<IShip>()).Returns(_thrusterArray);

            _position = new Vector2(12f, 5.5f);
            _ship = new Ship("TestShip", _position, 16f, Color.Goldenrod, _graphics, _shipComponentFactory)
            {
                Controller = _controller
            };
        }

        [Test]
        public void Ship_can_be_created_and_its_properties_are_set_in_the_constructor()
        {
            Assert.AreEqual("TestShip", _ship.Name);
            Assert.AreEqual(Color.Goldenrod, _ship.Color);

            Assert.AreEqual(_position, _ship.Position);
            Assert.AreEqual(16f, _ship.Radius);
            Assert.AreEqual(1f, _ship.Mass);

            _shipComponentFactory.Received(1).CreateEnergyStore();
            _shipComponentFactory.Received(1).CreateSheild(_ship);
            _shipComponentFactory.Received(1).CreateHull(_ship);
            _shipComponentFactory.Received(1).CreateThrusterArray(_ship);
        }

        [Test]
        public void Energy_returns_Level_from_EnergyStore()
        {
            _energyStore.Level.Returns(99f);

            Assert.AreEqual(99f, _ship.Energy);
            var temp = _energyStore.Received().Level;
            Assert.AreEqual(0f, temp); //pointless test but keeps compiler and resharper happy
        }

        [Test]
        public void Shields_returns_Level_from_Shield()
        {
            _shield.Level.Returns(55f);

            Assert.AreEqual(55f, _ship.Shields);
            var temp = _shield.Received().Level;
            Assert.AreEqual(0f, temp); //pointless test but keeps compiler and resharper happy
        }

        [Test]
        public void Armour_returns_Level_from_Hull()
        {
            _hull.Level.Returns(19f);

            Assert.AreEqual(19f, _ship.Armour);
            var temp = _hull.Received().Level;
            Assert.AreEqual(0f, temp); //pointless test but keeps compiler and resharper happy
        }

        [Test]
        public void Update_calls_relevant_methods_on_components()
        {
            var gameTime = new GameTime(new TimeSpan(0, 0, 1, 0), new TimeSpan(0, 0, 0, 0, 20));
            _ship.Update(gameTime);

            _shield.Received(1).Recharge(0.02f);
            _energyStore.Received(1).Recharge(0.02f);

            _controller.Received(1).GetAction();
            _thrusterArray.ReceivedWithAnyArgs(1).CalculateThrustPattern(Arg.Any<ShipAction>());
            _thrusterArray.Received(1).EngageThrusters();
        }

        [Test]
        public void Damage_passes_on_damage_to_shields()
        {
            _ship.Damage(10f);
            _shield.Received(1).Damage(10f);
        }

        [Test]
        public void Damage_passes_on_damage_to_hull_if_sheilds_return_a_value()
        {
            _shield.Damage(10f).Returns(6f);
            _hull.Level.Returns(94f);

            _ship.Damage(10f);
            
            Assert.IsFalse(_ship.Expired);
            _shield.Received(1).Damage(10f);
            _hull.Received(1).Damage(6f);
        }

        [Test]
        public void Damage_will_expire_the_ship_if_hull_Level_less_than_zero()
        {
            _shield.Damage(10f).Returns(6f);
            _hull.Level.Returns(-4f);

            _ship.Damage(10f);

            Assert.IsTrue(_ship.Expired);
            _shield.Received(1).Damage(10f);
            _hull.Received(1).Damage(6f);
        }

        [Test]
        public void RequestEnergy_passes_on_request_to_EnergyStore()
        {
            _ship.RequestEnergy(13f);

            _energyStore.Received(1).RequestEnergy(13f);
        }
    }
}
