using NUnit.Framework;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2.GameObjects;

using Microsoft.Xna.Framework;
using System;
using NSubstitute;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class GravitySimulatorTests
    {
        private GravitySimulator _gravitySimulator;
        private IGameObject _participant;
        private IGameObject _source;

        [SetUp]
        public void Setup()
        {
            _gravitySimulator = new GravitySimulator();

            _participant = Substitute.For<IGameObject>();
            _source = Substitute.For<IGameObject>();

            _source.Radius.Returns(20f);
            _source.Position.Returns(new Vector2(0, 0));
            _source.Mass.Returns(100f);

            _participant.Radius.Returns(5f);
            _participant.Position.Returns(new Vector2(50, 50));
            _participant.Mass.Returns(10f);
        }

        [Test]
        public void Simulate_Applies_External_Force_To_Participant_But_Not_To_Source()
        {
            _gravitySimulator.RegisterParticipant(_participant);
            _gravitySimulator.RegisterSource(_source);

            _gravitySimulator.Simulate();

            _participant.Received().ApplyExternalForce(Arg.Any<Force>());
            _participant.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());

            _source.DidNotReceive().ApplyExternalForce(Arg.Any<Force>());
            _source.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());


        }

    }
}
