using System;
using DEMW.SpaceWar2.Physics;

namespace DEMW.SpaceWar2Tests.TestUtils
{
    static public class ComparisonHelper
    {
        public static bool Matches(this Force expectedForce, Force actualForce)
        {
            return Matches(expectedForce.Displacement.X, actualForce.Displacement.X) &&
                   Matches(expectedForce.Displacement.Y, actualForce.Displacement.Y) &&
                   Matches(expectedForce.Vector.X, actualForce.Vector.X) &&
                   Matches(expectedForce.Vector.Y, actualForce.Vector.Y);
        }

        public static bool Matches(this float actual, float expected)
        {
            const float EPSILON = 0.00001f;

            bool matchFloat = Math.Abs(expected - actual) < EPSILON;
            return matchFloat;
        }
    }
}