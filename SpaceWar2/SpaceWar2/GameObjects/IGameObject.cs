using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{
    interface IGameObject
    {
        bool Expired { get; }

        Vector2 Position { get; set; }
        Vector2 Velocity { set; }
        
        float Mass { get; set; }
        float Radius { get; set; }
        
        void ApplyForce(Force force);
        Vector2 ResolveForces();

        void Teleport(Vector2 destination);
        
        void Draw();
    }
}
