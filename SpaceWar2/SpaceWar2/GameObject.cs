using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceWar2
{
    internal abstract class GameObject : IGameObject
    {
        protected readonly IList<Vector2> Forces;
        internal GameObject (Vector2 position, float radius, float mass)
        {
            Position = position;
            Radius = radius;
            Mass = mass;

            Forces = new List<Vector2>();
        }

        public bool Expired { get; protected set; }
        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public float Radius { get; set; }
        
        public void ApplyForce(Vector2 force)
        {
            Forces.Add(force);
        }

        public void ResolveForces()
        {
            Forces.Clear();
        }

        public abstract void Draw();
    }
}
