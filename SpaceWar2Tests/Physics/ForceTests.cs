using System;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Physics
{
    [TestFixture]
    class ForceTests
    {
        private Vector2 _right;
        private Vector2 _up;
        private Force _dispRightVectorUp;
        private Force _noDispVectorRight;
        private Force _dispDiagonalRightDownVectorDiagonalUpRight;
        
        [SetUp]
        public void Setup()
        {
            _right = new Vector2(1,0);
            _up = new Vector2(0,-1);
            var root2over2 = (float)Math.Sqrt(2) / 2f;
            var downRight = new Vector2(root2over2, root2over2);
            var upRight = new Vector2(root2over2, -root2over2);
            
            _dispRightVectorUp = new Force(_up, _right);
            _noDispVectorRight = new Force(_right, Vector2.Zero);
            _dispDiagonalRightDownVectorDiagonalUpRight = new Force(upRight, downRight);
        }

        [Test]
        public void Rotate_by_small_angle_does_not_affect_zero_Displacement()
        {
            _noDispVectorRight.Rotate(MathHelper.ToRadians(5));

            Assert.AreEqual(Vector2.Zero, _noDispVectorRight.Displacement);
        }

        [Test]
        public void Rotate_by_large_angle_does_not_affect_zero_Displacement()
        {
            _noDispVectorRight.Rotate(MathHelper.ToRadians(300));

            Assert.AreEqual(Vector2.Zero, _noDispVectorRight.Displacement);
        }

        [Test]
        public void Rotate_calculates_Displacement_and_Vector_correctly()
        {
            _dispRightVectorUp.Rotate(MathHelper.ToRadians(45));

            Assert.AreEqual(_dispDiagonalRightDownVectorDiagonalUpRight.Displacement, _dispRightVectorUp.Displacement, "Displacement");
            Assert.AreEqual(_dispDiagonalRightDownVectorDiagonalUpRight.Vector, _dispRightVectorUp.Vector, "Vector");
        }

        [Test]
        public void Force_default_constructor_creates_force_with_no_vector_and_no_displacement()
        {
            var defaultForce = new Force();

            Assert.AreEqual(Vector2.Zero, defaultForce.Vector);
            Assert.AreEqual(Vector2.Zero, defaultForce.Displacement);
        }

        [Test]
        public void Clone_creates_an_exact_copy_of_force()
        {
            var direction = new Vector2(0, -1);
            var disaplacment = new Vector2(1, 0);
            var force = new Force(direction, disaplacment);

            var forceClone = force.Clone();

            Assert.AreEqual(force.Vector, forceClone.Vector);
            Assert.AreEqual(force.Displacement, forceClone.Displacement);
            Assert.AreNotSame(force, forceClone);
        }
    }
}
