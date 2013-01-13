using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using DEMW.SpaceWar2Tests.TestUtils;
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
        private const float ThrustPower = ThrusterArray.ThrustPower;
        
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
            _thrusterArray.CalculateThrustPattern(ShipActions.None);

            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

        [Test]
        public void EngageThrusters_does_not_thrust_when_thrust_and_reverse_together()
        {
            _thrusterArray.CalculateThrustPattern(ShipActions.Thrust | ShipActions.ReverseThrust);
            _thrusterArray.EngageThrusters();

            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

        [Test]
        public void EngageThrusters_does_not_thrust_when_left_and_right_together()
        {
            _thrusterArray.CalculateThrustPattern(ShipActions.TurnLeft | ShipActions.TurnRight);
            _thrusterArray.EngageThrusters();

            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

        [Test]
        public void EngageThrusters_applies_forward_force_when_Thrust_pressed()
        {
            _ship.RequestEnergy(0.2f).Returns(0.2f);
            _thrusterArray.CalculateThrustPattern(ShipActions.Thrust);
            _thrusterArray.EngageThrusters();

            var forwardVector = new Vector2(0f, -ThrustPower);
            var expectedForceLeft = new Force(forwardVector, _leftThrusterPosition);
            var expectedForceRight = new Force(forwardVector, _rightThrusterPosition);

            _ship.Received().AngularVelocity = 0f;
            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceLeft)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceRight)));
        }

        [Test]
        public void EngageThrusters_applies_backwards_force_when_ReverseThrust_pressed()
        {
            _ship.RequestEnergy(0.2f).Returns(0.2f);
            _thrusterArray.CalculateThrustPattern(ShipActions.ReverseThrust);
            _thrusterArray.EngageThrusters();

            var backwardVector = new Vector2(0f, ThrustPower);
            var expectedForceLeft = new Force(backwardVector, _leftThrusterPosition);
            var expectedForceRight = new Force(backwardVector, _rightThrusterPosition);

            _ship.Received().AngularVelocity = 0f;
            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceLeft)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceRight)));
        }

        [Test]
        public void EngageThrusters_applies_suitable_rotational_forces_when_right_pressed()
        {
            _ship.RequestEnergy(0.05f).Returns(0.05f);
            _thrusterArray.CalculateThrustPattern(ShipActions.TurnRight);
            _thrusterArray.EngageThrusters();

            var forwardVector = new Vector2(0f, -ThrustPower * 0.25f);
            var backwardVector = new Vector2(0f, ThrustPower * 0.25f);
            var expectedForceLeft = new Force(forwardVector, _leftThrusterPosition);
            var expectedForceRight = new Force(backwardVector, _rightThrusterPosition);

            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceLeft)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceRight)));
        }

        [Test]
        public void EngageThrusters_applies_suitable_forces_when_forward_and_right_pressed()
        {
            _ship.RequestEnergy(Arg.Any<float>()).Returns(0.225f);
            _thrusterArray.CalculateThrustPattern(ShipActions.Thrust | ShipActions.TurnRight);
            _thrusterArray.EngageThrusters();

            var expectedForceFrontLeft = new Force(new Vector2(0f, -ThrustPower), _leftThrusterPosition);
            var expectedForceFrontRight = new Force(new Vector2(0f, -ThrustPower), _rightThrusterPosition);
            var expectedForceRearRight = new Force(new Vector2(0f, ThrustPower / 4f), _rightThrusterPosition);
            
            _ship.Received(1).RequestEnergy(Arg.Is<float>(x => x.Matches(0.225f)));
            _ship.Received(3).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceFrontLeft)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceFrontRight)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceRearRight)));
        }

        [Test]
        public void EngageThrusters_applies_suitable_rotational_forces_when_left_pressed()
        {
            _ship.RequestEnergy(0.05f).Returns(0.05f);
            _thrusterArray.CalculateThrustPattern(ShipActions.TurnLeft);
            _thrusterArray.EngageThrusters();

            var backwardVector = new Vector2(0f, ThrustPower * 0.25f);
            var forwardVector = new Vector2(0f, -ThrustPower * 0.25f);
            var expectedForceLeft = new Force(backwardVector, _leftThrusterPosition);
            var expectedForceRight = new Force(forwardVector, _rightThrusterPosition);

            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceLeft)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceRight)));
        }

        [Test]
        public void EngageThrusters_scales_back_all_thrusters_evenly_when_under_powered()
        {
            _ship.RequestEnergy(0.2f).Returns(0.1f);
            _thrusterArray.CalculateThrustPattern(ShipActions.Thrust);
            _thrusterArray.EngageThrusters();

            var forwardVector = new Vector2(0f, -ThrustPower * 0.5f);
            var expectedForceLeft = new Force(forwardVector, _leftThrusterPosition);
            var expectedForceRight = new Force(forwardVector, _rightThrusterPosition);
            
            _ship.Received().AngularVelocity = 0f;
            _ship.Received(2).ApplyInternalForce(Arg.Any<Force>());
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceLeft)));
            _ship.Received().ApplyInternalForce(Arg.Is<Force>(x => x.Matches(expectedForceRight)));
        }

        [Test]
        public void EngageThrusters_does_not_thrust_when_no_power_is_available()
        {
            _ship.RequestEnergy(0.2f).Returns(0.0f);
            _thrusterArray.CalculateThrustPattern(ShipActions.ReverseThrust);
            _thrusterArray.EngageThrusters();

            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }
    }
}
