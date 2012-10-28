using System;
using DEMW.SpaceWar2.Controls;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class ShipActionTests
    {
        [Test]
        public void ShipAction_none_is_zero()
        {
            Assert.AreEqual(0, (int)ShipAction.None);
        }

        [Test]
        public void ShipAction_has_flags_attribute()
        {
            Assert.IsNotNull(Attribute.GetCustomAttribute(typeof(ShipAction), typeof(FlagsAttribute)));
        }
    }
}
