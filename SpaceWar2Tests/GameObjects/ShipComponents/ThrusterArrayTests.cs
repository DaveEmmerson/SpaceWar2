using System.Linq;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using DEMW.SpaceWar2.Physics;
using NUnit.Framework;
using NSubstitute;

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
            _thrusterArray = new ThrusterArray(_ship);
            _ship.Radius.Returns(10);
            _ship.AngularVelocity.Returns(0);
        }

        [Test]
        public void CalculateThrustPattern_does_not_thrust_when_called_with_no_ShipActions()
        {
            _thrusterArray.CalculateThrustPattern(ShipAction.None);

            //Assertions
            _ship.Received().AngularVelocity = 0f;
            _ship.DidNotReceive().ApplyInternalForce(Arg.Any<Force>());
        }

    }
}
