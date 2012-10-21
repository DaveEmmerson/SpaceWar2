using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.GameObjects
{
    interface IGameObject
    {
        bool Expired { get; }

        float Mass { get; }
        Vector2 Position { get; }
        
        float MomentOfInertia { get; }
        float Rotation { get; }

        float Radius { get; }
        
        string ModelPath { get; }
        Model Model { get; set; }
        Color Color { get; set; }

        void ApplyForce(Force force);
        
        void Teleport(Vector2 destination);

        void Draw();
    }
}
