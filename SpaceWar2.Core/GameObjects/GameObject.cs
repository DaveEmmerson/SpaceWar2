using System.Collections.Generic;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.Core.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Core.GameObjects
{
    internal abstract class GameObject : IGameObject
    {
        private readonly IList<Force> _queuedforces;

        internal protected GameObject(Vector2 position, float radius, float mass)
        {
            Position = position;
            Radius = radius;
            Mass = mass;

            //Todo set this properly? - currently just a sphere
            MomentOfInertia = (2f * mass * radius * radius) / 5f;

            _queuedforces = new List<Force>();
            Forces = new List<Force>();
        }

        public bool Expired { get; protected set; }

        public float Mass { get; set; }
        public Vector2 Position { get; set; }
        internal Vector2 Velocity { get; set; }

        private float MomentOfInertia { get; set; }
        public float Rotation { get; set; }
        public float AngularVelocity { get; set; }

        public float Radius { get; set; }
        public Model Model { get; set; }
        public Color Color { get; set; }

        internal Vector2 TotalForce { get; private set; }
        internal float TotalMoment { get; private set; }
        
        protected IList<Force> Forces { get; private set; }

        public void ApplyExternalForce(Force force)
        {
            if (force == null || force.Vector == Vector2.Zero) return;
            
            var forceClone = force.Clone();
            _queuedforces.Add(forceClone);
        }

        public void ApplyInternalForce(Force force)
        {
            if (force == null || force.Vector == Vector2.Zero) return;

            var forceClone = force.Clone();
            forceClone.Rotate(Rotation);
            _queuedforces.Add(forceClone);
        }

        public void Teleport(Vector2 destination)
        {
            Position = destination;
        }

        internal void Update(GameTime gameTime)
        {
            var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateInternal(deltaT);
            SimulateDynamics(deltaT);
        }

        protected abstract void UpdateInternal(float deltaT);

        public abstract void Draw(IGraphicsDevice graphicsDevice);
        
        private void SimulateDynamics(float deltaT)
        {
            ResolveForces();
            CalculateVelocitiesAndPositions(deltaT);
        }

        private void ResolveForces()
        {
            Forces.Clear();
            TotalForce = Vector2.Zero;
            TotalMoment = 0f;

            foreach (var force in _queuedforces)
            {
                Forces.Add(force);
                TotalForce += force.Vector;
                TotalMoment += CalculateMoment(force);
            }

            _queuedforces.Clear();
        }

        private static float CalculateMoment(Force force)
        {
            if (force.Displacement == Vector2.Zero)
            {
                return 0f;
            }

            var radius = force.Displacement;
            var perpendicularToRadius = new Vector2(-radius.Y, radius.X);
            var moment = Vector2.Dot(perpendicularToRadius, force.Vector);
            return moment;
        }

        private void CalculateVelocitiesAndPositions(float deltaT)
        {
            var acceleration = (TotalForce / Mass);
            Velocity += acceleration * (deltaT / 2f);
            Position += Velocity * deltaT;
            Velocity += acceleration * (deltaT / 2f);

            var angularAcceleration = (TotalMoment / MomentOfInertia);
            AngularVelocity += angularAcceleration * (deltaT / 2f);
            Rotation += AngularVelocity * deltaT;
            AngularVelocity += angularAcceleration * (deltaT / 2f);
        }
    }
}
