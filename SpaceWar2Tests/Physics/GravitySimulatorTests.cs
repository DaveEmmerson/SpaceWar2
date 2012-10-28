using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

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
        public void Simulate_applies_correct_Force()
        {
            const int g = GravitySimulator.GravitationalConstant;

            _gravitySimulator.Simulate();

            // This checks that the force is diagonal 'up left', which is correct for
            // the setup specified in the mocks. The force magnitude I have calculated
            // manually. Would it be odd to calculate the magnitude based on the mock
            // values? It seems to just duplicate the code under test!
            // These comments should have been in a block, lol.
            _participant.Received(1).ApplyExternalForce(Arg.Is<Force>(
                force => force.Vector.Length() == g * 0.2 &&
                         force.Vector.X == force.Vector.Y &&
                         force.Vector.X < 0
            ));
        }

    }
}
