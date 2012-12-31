using System;
using System.Collections.Generic;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.GameObjects
{
    public abstract class GameObject : IGameObject
    {
        private readonly IList<Force> _queuedforces;

        protected GameObject (Vector2 position, float radius, float mass)
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
        public Vector2 Velocity { get; set; }

        private float MomentOfInertia { get; set; }
        public float Rotation { get; set; }
        public float AngularVelocity { get; set; }

        public float Radius { get; set; }
        public Model Model { get; set; }
        public Color Color { get; set; }
        
        public Vector2 TotalForce { get; private set; }
        public float TotalMoment { get; private set; }
        
        protected IList<Force> Forces { get; private set; }

        public void ApplyExternalForce(Force force)
        {
            if (force == null)
            {
                throw new ArgumentException("force must not be null.");
            }

            //TODO note that this method and the one below do not copy force, so any changes 
            //to force outside this class will not be without side effects
            if (force.Vector != Vector2.Zero)
            {
                _queuedforces.Add(force);
            }
        }

        public void ApplyInternalForce(Force force)
        {
            if (force.Vector == Vector2.Zero) return;
            
            //TODO Should we make a copy of force here instead of changing the original force?
            force.Rotate(Rotation);
            _queuedforces.Add(force);
        }

        public void Teleport(Vector2 destination)
        {
            Position = destination;
        }

        public void Update(GameTime gameTime)
        {
            var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateInternal(deltaT);
            SimulateDynamics(deltaT);
        }

        protected abstract void UpdateInternal(float deltaT);

        public abstract void Draw(IGraphicsDevice graphicsDevice = null);
        
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
