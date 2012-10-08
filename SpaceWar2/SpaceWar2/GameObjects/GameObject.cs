using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{
    internal abstract class GameObject : IGameObject
    {
        protected readonly IList<Vector2> Forces;
        private bool _forcesHaveBeenResolved;
        private Vector2 _resultantForce;

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
            if (_forcesHaveBeenResolved)
            {
                Forces.Clear();
                _forcesHaveBeenResolved = false;
            }

            if (force != Vector2.Zero)
            {
                Forces.Add(force);
            }
        }

        public Vector2 ResolveForces()
        {
            if (_forcesHaveBeenResolved == false)
            {
                _forcesHaveBeenResolved = true;
                _resultantForce = Forces.Aggregate(new Vector2(), (total, current) => total + current);
            }
           
            return _resultantForce;
        }

        public abstract void Draw();
    }
}
