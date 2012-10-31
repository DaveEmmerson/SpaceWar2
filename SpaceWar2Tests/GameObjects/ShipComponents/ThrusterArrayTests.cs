using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects.ShipComponents
{
    [TestFixture]
    class ThrustArrayTests
    {
        private IShip _ship;
        private ThrusterArray _thrusterArray;

        [SetUp]
        public void Setup()
        {
            _ship = Substitute.For<IShip>();
            _ship.Radius.Returns(10);
            _ship.AngularVelocity.Returns(0);
            _thrusterArray = new ThrusterArray(_ship);
        }

        [Test]
        public void EngageThrusters_does_not_thrust_when_called_with_no_ShipActions()
        {
            _thrusterArray.CalculateThrustPattern(ShipAction.None);

            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

        [Test]
        public void EngageThrusters_does_not_thrust_when_thrust_and_reverse_together()
        {
            _thrusterArray.CalculateThrustPattern(ShipAction.Thrust | ShipAction.ReverseThrust);
            _thrusterArray.EngageThrusters();

            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

        [Test]
        public void EngageThrusters_does_not_thrust_when_left_and_right_together()
        {
            _thrusterArray.CalculateThrustPattern(ShipAction.TurnLeft | ShipAction.TurnRight);
            _thrusterArray.EngageThrusters();

            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

        [Test]
        public void EngageThrusters_applies_forward_force_when_Thrust_pressed()
        {
            _ship.RequestEnergy(0.2f).Returns(0.2f);
            _thrusterArray.CalculateThrustPattern(ShipAction.Thrust);
            _thrusterArray.EngageThrusters();

            var leftThrusterPosition = new Vector2(-_ship.Radius, 0f);
            var rightThrusterPosition = new Vector2(_ship.Radius, 0f);
            var forwardVector = new Vector2(0f, -ThrusterArray.ThrustPower);

            _ship.Received().AngularVelocity = 0f;
            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == leftThrusterPosition && x.Vector == forwardVector));
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == rightThrusterPosition && x.Vector == forwardVector));
        }

        [Test]
        public void EngageThrusters_applies_backwards_force_when_ReverseThrust_pressed()
        {
            _ship.RequestEnergy(0.2f).Returns(0.2f);
            _thrusterArray.CalculateThrustPattern(ShipAction.ReverseThrust);
            _thrusterArray.EngageThrusters();

            var leftThrusterPosition = new Vector2(-_ship.Radius, 0f);
            var rightThrusterPosition = new Vector2(_ship.Radius, 0f);
            var forwardVector = new Vector2(0f, ThrusterArray.ThrustPower);

            _ship.Received().AngularVelocity = 0f;
            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == leftThrusterPosition && x.Vector == forwardVector));
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == rightThrusterPosition && x.Vector == forwardVector));
        }

        //TODO Add tests for rotation
        //TODO Add tests for when there is less power than requested
        //TODO Add tests for when no power
        //TODO Add tests for thrusting and turning together

    }
}
