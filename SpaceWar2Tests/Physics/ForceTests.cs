using System;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class ForceTests
    {
        private Force _dispRightVectorUp;
        private Force _noDispVectorRight;
        private Force _dispDiagonalRightDownVectorDiagonalUpRight;

        [SetUp]
        public void Setup()
        {
            var right = new Vector2(1,0);
            var up = new Vector2(0,-1);
            var root2over2 = (float)Math.Sqrt(2) / 2f;
            var downRight = new Vector2(root2over2, root2over2);
            var upRight = new Vector2(root2over2, -root2over2);
            
            _dispRightVectorUp = new Force(up, right);
            _noDispVectorRight = new Force(right, Vector2.Zero);
            _dispDiagonalRightDownVectorDiagonalUpRight = new Force(upRight, downRight);
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

        [Test]
        public void Offset_Force_Added_To_Opposite_Offset_Force_Gives_What()
        {
            Assert.Fail("I think there is a problem here. How can you combine these two forces?");

        }

        [Test]
        public void Rotate_By_Small_Angle_Does_Not_Affect_Zero_Displacement()
        {

            _noDispVectorRight.Rotate(MathHelper.ToRadians(5));

            Assert.AreEqual(Vector2.Zero, _noDispVectorRight.Displacement);

        }

        [Test]
        public void Rotate_By_Large_Angle_Does_Not_Affect_Zero_Displacement()
        {

            _noDispVectorRight.Rotate(MathHelper.ToRadians(300));

            Assert.AreEqual(Vector2.Zero, _noDispVectorRight.Displacement);

        }

        [Test]
        public void Rotate_Calculates_Displacement_And_Vector_Correctly()
        {
            _dispRightVectorUp.Rotate(MathHelper.ToRadians(45));

            Assert.AreEqual(_dispDiagonalRightDownVectorDiagonalUpRight.Displacement, _dispRightVectorUp.Displacement, "Displacement");
            Assert.AreEqual(_dispDiagonalRightDownVectorDiagonalUpRight.Vector, _dispRightVectorUp.Vector, "Vector");

        }

    }
}
