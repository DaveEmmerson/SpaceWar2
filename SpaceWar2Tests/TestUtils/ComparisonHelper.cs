using System;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2Tests.TestUtils
{
    static public class ComparisonHelper
    {
        public static bool Matches(this Force expectedForce, Force actualForce)
        {
            return Matches(expectedForce.Displacement, actualForce.Displacement) &&
                   Matches(expectedForce.Vector, actualForce.Vector);
        }

        public static bool Matches(this Vector2 expectedVector, Vector2 actualVector)
        {
            return Matches(expectedVector.X, actualVector.X) &&
                   Matches(expectedVector.Y, actualVector.Y);
        }

        public static bool Matches(this float actual, float expected)
        {
            const float EPSILON = 0.00001f;

            bool matchFloat = Math.Abs(expected - actual) < EPSILON;
            return matchFloat;
        }
    }
}