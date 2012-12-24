using System;
using DEMW.SpaceWar2.Physics;

namespace DEMW.SpaceWar2Tests.TestUtils
{
    static internal class ComparisonHelper
    {
        public static bool Matches(this Force expectedForce, Force actualForce)
        {
            return MatchFloat(expectedForce.Displacement.X, actualForce.Displacement.X) &&
                   MatchFloat(expectedForce.Displacement.Y, actualForce.Displacement.Y) &&
                   MatchFloat(expectedForce.Vector.X, actualForce.Vector.X) &&
                   MatchFloat(expectedForce.Vector.Y, actualForce.Vector.Y);
        }

        public static bool MatchFloat(this float actual, float expected)
        {
            const float EPSILON = 0.00001f;

            bool matchFloat = Math.Abs(expected - actual) < EPSILON;
            return matchFloat;
        }
    }
}