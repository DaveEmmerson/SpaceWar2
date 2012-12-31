using System;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects
{
    [TestFixture]
    internal class ShipTests
    {
        private IGraphicsFactory _graphicsFactory;
        private IGraphicsDevice _graphicsDevice;

        private IShipComponentFactory _shipComponentFactory;
        private IEnergyStore _energyStore;
        private IShield _shield;
        private IHull _hull;
        private IThrusterArray _thrusterArray;
        private IShipController _controller;

        private Vector2 _position;
        private Ship _ship;
        
        private const float _radius = 16f;

        [SetUp]
        public void SetUp()
        {
            _graphicsFactory = Substitute.For<IGraphicsFactory>();
            _graphicsDevice = Substitute.For<IGraphicsDevice>();
            _shipComponentFactory = Substitute.For<IShipComponentFactory>();

            _energyStore = Substitute.For<IEnergyStore>();
            _shield = Substitute.For<IShield>();
            _hull = Substitute.For<IHull>();
            _thrusterArray = Substitute.For<IThrusterArray>();
            _controller = Substitute.For<IShipController>();

            _shipComponentFactory.CreateEnergyStore().ReturnsForAnyArgs(_energyStore);
            _shipComponentFactory.CreateShield(Arg.Any<IShip>()).Returns(_shield);
            _shipComponentFactory.CreateHull(Arg.Any<IShip>()).Returns(_hull);
            _shipComponentFactory.CreateThrusterArray(Arg.Any<IShip>()).Returns(_thrusterArray);

            _position = new Vector2(12f, 5.5f);
            _ship = new Ship("TestShip", _position, _radius, Color.Goldenrod, _graphicsFactory, _shipComponentFactory)
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
            Assert.AreEqual(_radius, _ship.Radius);
            Assert.AreEqual(1f, _ship.Mass);

            _shipComponentFactory.Received(1).CreateEnergyStore();
            _shipComponentFactory.Received(1).CreateShield(_ship);
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
            var temp = _hull.Received(1).Level;
            Assert.AreEqual(0f, temp); //pointless test but keeps compiler and resharper happy
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
            var temp = _hull.Received(1).Level;
            Assert.AreEqual(0f, temp); //pointless test but keeps compiler and resharper happy
        }

        [Test]
        public void RequestEnergy_passes_on_request_to_EnergyStore()
        {
            _ship.RequestEnergy(13f);

            _energyStore.Received(1).RequestEnergy(13f);
        }

        [Test]
        public void Draw_does_nothing_if_ShowArrows_is_False()
        {
            _ship.ShowArrows = false;
            _ship.Draw(_graphicsDevice);

            _graphicsFactory.DidNotReceiveWithAnyArgs().CreateAccelerationArrow(Arg.Any<Vector2>(), Arg.Any<float>());
            _graphicsFactory.DidNotReceiveWithAnyArgs().CreateVelocityArrow(Arg.Any<Vector2>(), Arg.Any<float>());
            _graphicsFactory.DidNotReceiveWithAnyArgs().CreateRotationArrow(Arg.Any<float>(), Arg.Any<float>());
            _graphicsFactory.DidNotReceiveWithAnyArgs().CreateForceArrow(Arg.Any<Force>(), Arg.Any<float>());
        }

        [Test]
        public void Draw_creates_three_arrows_and_draws_them_when_ShowArrows_is_True()
        {
            var accelerationArrow = Substitute.For<IArrow>();
            var velocityArrow = Substitute.For<IArrow>();
            var rotationArrow = Substitute.For<IArrow>();
            _graphicsFactory.CreateAccelerationArrow(Arg.Any<Vector2>(), Arg.Any<float>()).Returns(accelerationArrow);
            _graphicsFactory.CreateVelocityArrow(Arg.Any<Vector2>(), Arg.Any<float>()).Returns(velocityArrow);
            _graphicsFactory.CreateRotationArrow(Arg.Any<float>(), Arg.Any<float>()).Returns(rotationArrow);
            
            _ship.ShowArrows = true;
            _ship.Velocity = new Vector2(1f, 1f);
            _ship.Rotation = 1f;
            _ship.Draw(_graphicsDevice);

            _graphicsFactory.Received(1).CreateAccelerationArrow(new Vector2(0f, 0f), _radius);
            _graphicsFactory.Received(1).CreateVelocityArrow(new Vector2(1f, 1f), _radius);
            _graphicsFactory.Received(1).CreateRotationArrow(1f, _radius);
            _graphicsFactory.DidNotReceiveWithAnyArgs().CreateForceArrow(Arg.Any<Force>(), Arg.Any<float>());

            accelerationArrow.Received(1).Draw(_graphicsDevice);
            velocityArrow.Received(1).Draw(_graphicsDevice);
            rotationArrow.Received(1).Draw(_graphicsDevice);
        }

        [Test]
        public void Draw_creates_arrows_for_each_force_and_draws_them_when_ShowArrows_is_True()
        {
            var accelerationArrow = Substitute.For<IArrow>();
            var velocityArrow = Substitute.For<IArrow>();
            var rotationArrow = Substitute.For<IArrow>();
            var force1Arrow = Substitute.For<IArrow>();
            var force2Arrow = Substitute.For<IArrow>();

            var force1 = new Force(new Vector2(2.5f, 0.1f), new Vector2(0f,0f));
            var force2 = new Force(new Vector2(0.1f, 9.9f), new Vector2(-1f,-1f));

            _graphicsFactory.CreateAccelerationArrow(Arg.Any<Vector2>(), Arg.Any<float>()).Returns(accelerationArrow);
            _graphicsFactory.CreateVelocityArrow(Arg.Any<Vector2>(), Arg.Any<float>()).Returns(velocityArrow);
            _graphicsFactory.CreateRotationArrow(Arg.Any<float>(), Arg.Any<float>()).Returns(rotationArrow);
            _graphicsFactory.CreateForceArrow(force1, Arg.Any<float>()).Returns(force1Arrow);
            _graphicsFactory.CreateForceArrow(force2, Arg.Any<float>()).Returns(force2Arrow);
            
            _ship.ShowArrows = true;
            _ship.ApplyInternalForce(force1);
            _ship.ApplyExternalForce(force2);
            _ship.Update(new GameTime());
            _ship.Draw(_graphicsDevice);

            _graphicsFactory.Received(1).CreateAccelerationArrow(new Vector2(2.6f, 10.0f), _radius);
            _graphicsFactory.Received(1).CreateVelocityArrow(new Vector2(0f, 0f), _radius);
            _graphicsFactory.Received(1).CreateRotationArrow(0f, _radius);
            _graphicsFactory.Received(1).CreateForceArrow(force1, _radius);
            _graphicsFactory.Received(1).CreateForceArrow(force2, _radius);

            accelerationArrow.Received(1).Draw(_graphicsDevice);
            velocityArrow.Received(1).Draw(_graphicsDevice);
            rotationArrow.Received(1).Draw(_graphicsDevice);
            force1Arrow.Received(1).Draw(_graphicsDevice);
            force2Arrow.Received(1).Draw(_graphicsDevice);
        }

        [Test]
        public void DebugInfo_returns_a_string_containing_information_about_the_Ship()
        {
            _shield.Level.Returns(99.9f);
            _hull.Level.Returns(56f);
            _energyStore.Level.Returns(72f);
            _ship.Rotation = 1.3f;
            _ship.Velocity = new Vector2(1.5f,-2.9f);
            _ship.AngularVelocity = 0.0001f;
            
            var actualMessage = _ship.DebugDetails;

            const string expectedMessage =
@"TestShip.Position.X: 12
TestShip.Position.Y: 5.5
TestShip.Velocity.X: 1.5
TestShip.Velocity.Y: -2.9

TestShip.Rotation: 1.3
TestShip.AngularVelocity: 0.0001

TestShip.Shields: 99.9
TestShip.Armour: 56
TestShip.Energy: 72
";

            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
