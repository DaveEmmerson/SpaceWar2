using NUnit.Framework;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class ForceTests
    {
        private Force _dispRightVectorUp;
        private Force _noDispVectorRight;

        [SetUp]
        public void Setup()
        {
            _dispRightVectorUp = new Force(displacement:new Vector2(1, 0), force:new Vector2(0, -1));
            _noDispVectorRight = new Force(displacement:new Vector2(0, 0), force:new Vector2(1, 0));
        }

        [Test]
        public void Offset_Force_Added_To_Self_Gives_Same_Displacement_And_Double_The_Vector()
        {
            var result = _dispRightVectorUp + _dispRightVectorUp;

            Assert.AreEqual(_dispRightVectorUp.Displacement, result.Displacement, "Displacement");

            Assert.AreEqual(_dispRightVectorUp.Vector * 2, result.Vector, "Vector");

        }

        [Test]
        public void Centre_Force_Added_To_Self_Gives_Zero_Displacement_And_Double_The_Vector()
        {
            var result = _noDispVectorRight + _noDispVectorRight;

            Assert.AreEqual(Vector2.Zero, result.Displacement, "Displacement");

            Assert.AreEqual(_noDispVectorRight.Vector * 2, result.Vector, "Vector");
        }
    }
}
