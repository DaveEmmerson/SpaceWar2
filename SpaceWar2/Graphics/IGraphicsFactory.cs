using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    public interface IGraphicsFactory
    {
        Arrow CreateAccelerationArrow(Vector2 acceleration, float radius);
        Arrow CreateVelocityArrow(Vector2 direction, float radius);
        Arrow CreateRotationArrow(Vector2 direction, float radius);
        Arrow CreateForceArrow(Vector2 position, Vector2 direction, float radius);
    }
}