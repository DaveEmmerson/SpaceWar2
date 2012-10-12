using System.Collections.Generic;
using System.Linq;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{
    internal abstract class GameObject : IGameObject
    {
        private readonly IList<Force> _queuedforces;
        private Vector2 _resultantForce;

        internal GameObject (Vector2 position, float radius, float mass)
        {
            Position = position;
            Radius = radius;
            Mass = mass;

            _queuedforces = new List<Force>();
            Forces = new List<Force>();
        }

        public bool Expired { get; protected set; }
        
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        
        public float Rotation { get; set; }
        public float AngularVelocity { get; set; }

        public float Mass { get; set; }
        public float Radius { get; set; }
        
        public void ApplyForce(Force force)
        {
            if (force.Vector != Vector2.Zero)
            {
                _queuedforces.Add(force);
            }
        }

        public Vector2 ResolvedForce
        {
            get { return _resultantForce; }
        }

        protected IList<Force> Forces { get; private set; }

        public void Teleport(Vector2 destination)
        {
            Position = destination;
        }

        public void Update(GameTime gameTime)
        {
            UpdateInternal(gameTime);
            ResolveForces();
        }

        private void ResolveForces()
        {
            Forces.Clear();
            _resultantForce = Vector2.Zero;
            
            foreach (var force in _queuedforces)
            {
                Forces.Add(force);
                _resultantForce += force.Vector;
            }

            _queuedforces.Clear();
        }

        protected abstract void UpdateInternal(GameTime gameTime);

        public abstract void Draw();
    }
}
