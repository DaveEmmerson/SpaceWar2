using System;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2Tests.TestUtils;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class GravitySimulatorTests
    {
        private IGameObject _source;
        private IGameObject _participant;
        private GravitySimulator _gravitySimulator;
        
        [SetUp]
        public void Setup()
        {
            _source = Substitute.For<IGameObject>();
            _source.Radius.Returns(20f);
            _source.Position.Returns(new Vector2(0, 0));
            _source.Mass.Returns(100f);

            _participant = Substitute.For<IGameObject>();
            _participant.Radius.Returns(5f);
            _participant.Position.Returns(new Vector2(50, 50));
            _participant.Mass.Returns(10f);

            _gravitySimulator = new GravitySimulator();
            _gravitySimulator.RegisterParticipant(_participant);
            _gravitySimulator.RegisterSource(_source);
        }

        [Test]
        public void Simulate_applies_external_Force_to_participant_but_not_to_source()
        {
            _gravitySimulator.Simulate();

            _participant.Received(1).ApplyExternalForce(Arg.Any<Force>());
            _participant.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());

            _source.DidNotReceive().ApplyExternalForce(Arg.Any<Force>());
            _source.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

        [Test]
        public void Simulate_applies_correct_Force_for_non_collision_case()
        {
            _gravitySimulator.Simulate();

            // This checks that the force is diagonal 'up left', which is correct for
            // the setup specified in the mocks. The force direction vector is 
            // calculated based on the force expected given the values provided in 
            // the mock set-up.
            const int h = GravitySimulator.GravitationalConstant / 5;
            var side = 0 - (float)Math.Sqrt((h * h) / 2f);

            var expectedForce = new Force(new Vector2(side, side));
            _participant.Received(1).ApplyExternalForce(Arg.Is<Force>(force => force.Matches(expectedForce)));
        }

        [Test]
        public void Simulate_applies_correct_Force_for_collision_case()
        {
            _participant.Radius.Returns(100);
            _gravitySimulator.Simulate();

            const int h = GravitySimulator.GravitationalConstant / 5;
            var side = (float)Math.Sqrt((h * h) / 2f);

            var expectedForce = new Force(new Vector2(side, side));
            _participant.Received(1).ApplyExternalForce(Arg.Is<Force>(force => force.Matches(expectedForce)));
        }

        [Test]
        public void UnRegister_removes_source_GameObject_from_GravitySimulator()
        {
            _gravitySimulator.Unregister(_source);
            _gravitySimulator.Simulate();

            _participant.DidNotReceive().ApplyExternalForce(Arg.Any<Force>());
            _participant.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());

            _source.DidNotReceive().ApplyExternalForce(Arg.Any<Force>());
            _source.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

        [Test]
        public void UnRegister_removes_participant_GameObject_from_GravitySimulator()
        {
            _gravitySimulator.Unregister(_participant);
            _gravitySimulator.Simulate();

            _participant.DidNotReceive().ApplyExternalForce(Arg.Any<Force>());
            _participant.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());

            _source.DidNotReceive().ApplyExternalForce(Arg.Any<Force>());
            _source.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }


    }
}
