using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2Tests.TestUtils;
using DEMW.SpaceWar2Tests.Utils;
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
        private Vector2 _leftThrusterPosition;
        private Vector2 _rightThrusterPosition;

        [SetUp]
        public void Setup()
        {
            _ship = Substitute.For<IShip>();
            _ship.Radius.Returns(10);
            _ship.AngularVelocity.Returns(0);
            _thrusterArray = new ThrusterArray(_ship);

            _leftThrusterPosition = new Vector2(-_ship.Radius, 0f);
            _rightThrusterPosition = new Vector2(_ship.Radius, 0f);
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

            var forwardVector = new Vector2(0f, -ThrusterArray.ThrustPower);

            _ship.Received().AngularVelocity = 0f;
            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _leftThrusterPosition && x.Vector == forwardVector));
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _rightThrusterPosition && x.Vector == forwardVector));
        }

        [Test]
        public void EngageThrusters_applies_backwards_force_when_ReverseThrust_pressed()
        {
            _ship.RequestEnergy(0.2f).Returns(0.2f);
            _thrusterArray.CalculateThrustPattern(ShipAction.ReverseThrust);
            _thrusterArray.EngageThrusters();

            var forwardVector = new Vector2(0f, ThrusterArray.ThrustPower);

            _ship.Received().AngularVelocity = 0f;
            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _leftThrusterPosition && x.Vector == forwardVector));
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _rightThrusterPosition && x.Vector == forwardVector));
        }

        [Test]
        public void EngageThrusters_applies_suitable_rotational_forces_when_right_pressed()
        {
            _ship.RequestEnergy(0.05f).Returns(0.05f);
            _thrusterArray.CalculateThrustPattern(ShipAction.TurnRight);
            _thrusterArray.EngageThrusters();

            var forwardVector = new Vector2(0f, -ThrusterArray.ThrustPower * 0.25f);
            var backwardVector = new Vector2(0f, ThrusterArray.ThrustPower * 0.25f);

            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _leftThrusterPosition && x.Vector == forwardVector));
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _rightThrusterPosition && x.Vector == backwardVector));
        }

        [Test]
        public void EngageThrusters_applies_suitable_forces_when_forward_and_right_pressed()
        {
            _ship.RequestEnergy(Arg.Any<float>()).Returns(0.225f);
            _thrusterArray.CalculateThrustPattern(ShipAction.Thrust | ShipAction.TurnRight);
            _thrusterArray.EngageThrusters();

            var expectedForceFrontLeft = new Force(new Vector2(0f, -ThrusterArray.ThrustPower), _leftThrusterPosition);
            var expectedForceFrontRight = new Force(new Vector2(0f, -ThrusterArray.ThrustPower), _rightThrusterPosition);
            var expectedForceRearRight = new Force(new Vector2(0f, ThrusterArray.ThrustPower / 4f), _rightThrusterPosition);
            
            _ship.Received(1).RequestEnergy(Arg.Is<float>(x => x.MatchFloat(0.225f)));
            _ship.Received(3).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceFrontLeft)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceFrontRight)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceRearRight)));
        }

        [Test]
        public void EngageThrusters_applies_suitable_rotational_forces_when_left_pressed()
        {
            _ship.RequestEnergy(0.05f).Returns(0.05f);
            _thrusterArray.CalculateThrustPattern(ShipAction.TurnLeft);
            _thrusterArray.EngageThrusters();

            var forwardVector = new Vector2(0f, -ThrusterArray.ThrustPower * 0.25f);
            var backwardVector = new Vector2(0f, ThrusterArray.ThrustPower * 0.25f);

            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _leftThrusterPosition && x.Vector == backwardVector));
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _rightThrusterPosition && x.Vector == forwardVector));
        }

        [Test]
        public void EngageThrusters_scales_back_all_thrusters_evenly_when_under_powered()
        {
            _ship.RequestEnergy(0.2f).Returns(0.1f);
            _thrusterArray.CalculateThrustPattern(ShipAction.Thrust);
            _thrusterArray.EngageThrusters();

            var forwardVector = new Vector2(0f, -ThrusterArray.ThrustPower * 0.5f);

            _ship.Received().AngularVelocity = 0f;
            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _leftThrusterPosition && x.Vector == forwardVector));
            _ship.Received().ApplyInternalForce(
                Arg.Is<Force>(x => x.Displacement == _rightThrusterPosition && x.Vector == forwardVector));
        }

        [Test]
        public void EngageThrusters_does_not_thrust_when_no_power_is_available()
        {
            _ship.RequestEnergy(0.2f).Returns(0.0f);
            _thrusterArray.CalculateThrustPattern(ShipAction.ReverseThrust);
            _thrusterArray.EngageThrusters();

            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }
    }
}
