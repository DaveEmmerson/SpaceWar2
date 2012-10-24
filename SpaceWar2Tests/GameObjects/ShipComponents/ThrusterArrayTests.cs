using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects.ShipComponents
{
    [TestFixture]
    class ThrustArrayTests
    {
        private Ship _ship;
        private ThrusterArray _thrusterArray;

        [SetUp]
        public void Setup()
        {
            _ship = new Ship("Test ship", null, Vector2.Zero, 10, Color.AliceBlue);
            _thrusterArray = new ThrusterArray(_ship);
        }

        [Test]
        public void CalculateThrustPattern_does_not_thrust_when_called_with_no_ShipActions()
        {
            _thrusterArray.CalculateThrustPattern(ShipAction.None);

        }

    }
}
