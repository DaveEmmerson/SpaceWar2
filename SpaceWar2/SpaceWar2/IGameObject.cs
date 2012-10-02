using Microsoft.Xna.Framework;

namespace SpaceWar2
{
    interface IGameObject
    {
        bool Expired { get; }

        Vector2 Position { get; set; }
        float Mass { get; set; }
        float Radius { get; set; }

        void Draw();
    }
}
