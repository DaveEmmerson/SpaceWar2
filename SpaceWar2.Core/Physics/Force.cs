using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Core.Physics
{
    public class Force
    {
        internal Force() : this(Vector2.Zero, Vector2.Zero) { }

        internal Force(Vector2 force) : this(force, Vector2.Zero) { }

        internal Force(Vector2 force, Vector2 displacement)
        {
            Vector = force;
            Displacement = displacement;
        }

        internal Vector2 Vector { get; private set; }
        internal Vector2 Displacement { get; private set; }

        internal void Rotate(float angle)
        {
            var rotation = Matrix.CreateRotationZ(angle);
            Vector = Vector2.Transform(Vector, rotation);
            Displacement = Vector2.Transform(Displacement, rotation);
        }

        internal Force Clone()
        {
            return new Force(Vector, Displacement);
        }
    }
}
