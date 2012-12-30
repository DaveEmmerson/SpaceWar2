using DEMW.SpaceWar2.Graphics;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class NullArrowTests
    {
        [Test]
        public void NullArrow_can_be_created_but_does_nothing()
        {
            var arrow = new NullArrow();
            Assert.DoesNotThrow(() => arrow.Draw(null));
        }
    }
}
