using NUnit.Framework;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class ForceTests
    {
        private Force _dispRightVectorUp;

        [SetUp]
        public void Setup()
        {
            _dispRightVectorUp = new Force(new Vector2(1, 0), new Vector2(0, -1));
        }

        [Test]
        public void Offset_Force_Added_To_Self_Gives_Same_Displacement_And_Double_The_Vector()
        {
            var result = _dispRightVectorUp + _dispRightVectorUp;

            Assert.AreEqual(_dispRightVectorUp.Displacement, result.Displacement);

            Assert.AreEqual(_dispRightVectorUp.Vector * 2, result.Vector);

        }
    }
}
