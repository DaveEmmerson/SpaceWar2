using Microsoft.Xna.Framework;

namespace SpaceWar2
{
    internal abstract class GameObject : IGameObject
    {
        internal GameObject (Vector2 position, float radius, float mass)
        {
            Position = position;
            Radius = radius;
            Mass = mass;
        }

        public bool Expired { get; protected set; }
        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public float Radius { get; set; }
        public abstract void Draw();
    }
}
