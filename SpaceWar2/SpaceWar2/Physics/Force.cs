using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Physics
{
    internal class Force
    {
        internal Force(Vector2 force) : this(force, Vector2.Zero)
        {
            Vector = force;
        }

        internal Force(Vector2 force, Vector2 displacement)
        {
            Vector = force;
            Displacement = displacement;
        }

        internal Vector2 Vector { get; private set; }
        internal Vector2 Displacement { get; private set; }
    }
}
