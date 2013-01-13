using System;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.TestUtils
{
    static public class ComparisonHelper
    {
        public static bool Matches(this Force actualForce, Force expectedForce)
        {
            return Matches(actualForce.Displacement.X, expectedForce.Displacement.X) &&
                   Matches(actualForce.Displacement.Y, expectedForce.Displacement.Y) &&
                   Matches(actualForce.Vector.X, expectedForce.Vector.X) &&
                   Matches(actualForce.Vector.Y, expectedForce.Vector.Y);
        }

        public static bool Matches(this float actual, float expected)
        {
            const float EPSILON = 0.00001f;

            return Math.Abs(expected - actual) < EPSILON;
        }

        public static void AssertAreEqualWithinTolerance(this Force actualForce, Force expectedForce)
        {
            AssertAreEqualWithinTolerance(expectedForce.Displacement, actualForce.Displacement);
            AssertAreEqualWithinTolerance(expectedForce.Vector, actualForce.Vector);
        }

        public static void AssertAreEqualWithinTolerance(this Vector2 actualVector, Vector2 expectedVector)
        {
            Assert.AreEqual(expectedVector.X, actualVector.X);
            Assert.AreEqual(expectedVector.Y, actualVector.Y);
        }

        public static void AssertAreEqualWithinTolerance(this Vector2 actualVector, Vector2 expectedVector, float delta)
        {
            actualVector.X.AssertAreEqualWithinTolerance(expectedVector.X, delta);
            actualVector.Y.AssertAreEqualWithinTolerance(expectedVector.Y, delta);
        }

        public static void AssertAreEqualWithinTolerance(this float actualFloat, float expectedFloat, float delta)
        {
            Assert.AreEqual(expectedFloat, actualFloat, delta);
        }
    }
}