using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{
    interface IGameObject
    {
        bool Expired { get; }

        Vector2 Position { get; }
        
        float Mass { get; }
        float Radius { get; }
        float Rotation { get; }

        void ApplyForce(Force force);
        
        void Teleport(Vector2 destination);
        
        void Draw();
    }
}
