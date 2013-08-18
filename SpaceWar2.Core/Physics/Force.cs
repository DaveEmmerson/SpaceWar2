using Microsoft.Xna.Framework;
using F = DEMW.SpaceWar2.FCore.Physics.Physics;

namespace DEMW.SpaceWar2.Core.Physics
{
    public class Force
    {
        private F.Force _force;
        
        internal Force() : this(Vector2.Zero, Vector2.Zero) { }

        internal Force(Vector2 force) : this(force, Vector2.Zero) { }

        internal Force(Vector2 force, Vector2 displacement)
        {
            _force = new F.Force(force, displacement);          
        }

        internal Vector2 Vector 
        {
            get { return _force.vector; }
        }

        internal Vector2 Displacement
        { 
            get { return _force.displacement; }
        }

        internal void Rotate(float angle)
        {
            _force = F.rotate(_force, angle);
        }
    }
}
