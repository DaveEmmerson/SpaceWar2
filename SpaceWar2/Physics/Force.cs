using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Physics
{
    public class Force
    {
        public Force() : this(Vector2.Zero, Vector2.Zero) { }
        
        public Force(Vector2 force) : this(force, Vector2.Zero) { }

        public Force(Vector2 force, Vector2 displacement)
        {
            Vector = force;
            Displacement = displacement;
        }

        public Vector2 Vector { get; private set; }
        public Vector2 Displacement { get; private set; }

        public static Force operator + (Force f1, Force f2)
        {
            return new Force(f1.Vector + f2.Vector, f1.Displacement + f2.Displacement);
        }

        public void Rotate(float angle)
        {
            var rotation = Matrix.CreateRotationZ(angle);
            Vector = Vector2.Transform(Vector, rotation);
            Displacement = Vector2.Transform(Displacement, rotation);
        }
    }
}
