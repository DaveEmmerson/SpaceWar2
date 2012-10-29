﻿using DEMW.SpaceWar2.Controls;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
	class NullShipControllerTests
	{
        [Test]
        public void GetAction_returns_none()
        {
            var nullController = new NullShipController();

            var action = nullController.GetAction();

            Assert.AreEqual(ShipAction.None, action);
        }
	}
}