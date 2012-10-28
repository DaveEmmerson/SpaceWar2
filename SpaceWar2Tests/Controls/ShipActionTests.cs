using System;
using DEMW.SpaceWar2.Controls;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class ShipActionTests
    {
        [Test]
        public void ShipAction_None_Is_Zero()
        {
            Assert.AreEqual(0, (int)ShipAction.None);
        }

        [Test]
        public void ShipAction_Has_Flags_Attribute()
        {
            Assert.IsNotNull(Attribute.GetCustomAttribute(typeof(ShipAction), typeof(FlagsAttribute)));
        }
    }
}
