using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceWar2
{
    internal abstract class GameObject : IGameObject
    {
        private readonly IList<Vector2> _forces;
        internal GameObject (Vector2 position, float radius, float mass)
        {
            Position = position;
            Radius = radius;
            Mass = mass;

            _forces = new List<Vector2>();
        }

        public bool Expired { get; protected set; }
        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public float Radius { get; set; }
        
        public void ApplyForce(Vector2 force)
        {
            _forces.Add(force);
        }

        public void ResolveForces()
        {
            _forces.Clear();
        }

        public abstract void Draw();
    }
}
