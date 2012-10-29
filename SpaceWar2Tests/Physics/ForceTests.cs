﻿using System;
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
        public void Offset_Force_added_to_self_gives_same_Displacement_and_double_the_Vector()
        {
            var result = _dispRightVectorUp + _dispRightVectorUp;

            Assert.AreEqual(_dispRightVectorUp.Displacement, result.Displacement, "Displacement");

            Assert.AreEqual(_dispRightVectorUp.Vector * 2, result.Vector, "Vector");

        }

        [Test]
        public void Centre_Force_added_to_self_gives_zero_Displacement_and_double_the_Vector()
        {
            var result = _noDispVectorRight + _noDispVectorRight;

            Assert.AreEqual(Vector2.Zero, result.Displacement, "Displacement");

            Assert.AreEqual(_noDispVectorRight.Vector * 2, result.Vector, "Vector");
        }

        [Test]
        public void Offset_Force_added_to_opposite_offset_Force_gives_what()
        {
            Assert.Fail("I think there is a problem here. How can you combine these two forces?");
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

    }
}
