using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.GameObjects
{
    interface IGameObject
    {
        bool Expired { get; }

        Vector2 Position { get; }
        
        float Mass { get; }
        float Radius { get; }
        float Rotation { get; }

        string ModelPath { get; }
        Model Model { get; set; }
        Color Color { get; set; }

        void ApplyForce(Force force);
        
        void Teleport(Vector2 destination);

        void Draw();
    }
}
