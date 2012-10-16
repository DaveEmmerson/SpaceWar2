using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Physics
{
    internal class Force
    {
        internal Force(Vector2 force) : this(force, Vector2.Zero) { }

        internal Force(Vector2 force, Vector2 displacement)
        {
            Vector = force;
            Displacement = displacement;
        }

        internal Vector2 Vector { get; private set; }
        internal Vector2 Displacement { get; private set; }

        public static Force operator + (Force f1, Force f2)
        {
            return new Force(f1.Vector + f2.Vector, f1.Displacement + f2.Displacement);
        }
    }
}
