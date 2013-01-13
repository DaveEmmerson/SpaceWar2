using DEMW.SpaceWar2.Controls;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
	class NullShipControllerTests
	{
        [Test]
        public void Actions_returns_none()
        {
            var nullController = new NullShipController();

            var actions = nullController.Actions;

            Assert.AreEqual(ShipActions.None, actions);
        }
	}
}
