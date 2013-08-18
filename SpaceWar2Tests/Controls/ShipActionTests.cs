using DEMW.SpaceWar2.Core.Controls;
using NUnit.Framework;
using System;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class ShipActionsTests
    {
        [Test]
        public void ShipActions_none_is_zero()
        {
            Assert.AreEqual(0, (int)ShipActions.None);
        }

        [Test]
        public void ShipActions_has_flags_attribute()
        {
            Assert.IsNotNull(Attribute.GetCustomAttribute(typeof(ShipActions), typeof(FlagsAttribute)));
        }
    }
}
