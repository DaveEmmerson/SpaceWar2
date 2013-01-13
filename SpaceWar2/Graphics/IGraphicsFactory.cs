using DEMW.SpaceWar2.Core.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    public interface IGraphicsFactory
    {
        IArrow CreateAccelerationArrow(Vector2 acceleration, float radius);
        IArrow CreateVelocityArrow(Vector2 direction, float radius);
        IArrow CreateRotationArrow(float rotation, float radius);
        IArrow CreateForceArrow(Force force, float radius);
    }
}