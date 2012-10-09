using System;
using System.Collections.Generic;
using System.Linq;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{
    internal abstract class GameObject : IGameObject
    {
        protected readonly IList<Force> Forces;
        private bool _forcesHaveBeenResolved;
        private Vector2 _resultantForce;

        internal GameObject (Vector2 position, float radius, float mass)
        {
            Position = position;
            Radius = radius;
            Mass = mass;

            Forces = new List<Force>();
        }

        public bool Expired { get; protected set; }
        
        //TODO MW lock
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        
        public float Rotation { get; set; }
        public float Mass { get; set; }
        public float Radius { get; set; }
        
        public void ApplyForce(Force force)
        {
            if (_forcesHaveBeenResolved)
            {
                Forces.Clear();
                _forcesHaveBeenResolved = false;
            }

            if (force.Vector != Vector2.Zero)
            {
                Forces.Add(force);
            }
        }

        public Vector2 ResolveForces()
        {
            if (_forcesHaveBeenResolved == false)
            {
                _forcesHaveBeenResolved = true;
                _resultantForce = Forces.Aggregate(Vector2.Zero, (total, current) => total + current.Vector);
            }
           
            return _resultantForce;
        }

        public abstract void Draw();
    }
}
