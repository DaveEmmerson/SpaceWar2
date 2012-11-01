﻿using System.Collections.Generic;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.GameObjects
{
    public abstract class GameObject : IGameObject
    {
        private readonly IList<Force> _queuedforces;

        internal GameObject (Vector2 position, float radius, float mass)
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

        public float MomentOfInertia { get; set; }
        public float Rotation { get; set; }
        public float AngularVelocity { get; set; }

        public float Radius { get; set; }

        public Model Model { get; set; }
        public Color Color { get; set; }
        
        public void ApplyExternalForce(Force force)
        {
            if (force.Vector != Vector2.Zero)
            {
                _queuedforces.Add(force);
            }
        }

        public void ApplyInternalForce(Force force)
        {
            if (force.Vector != Vector2.Zero)
            {
                force.Rotate(Rotation);
                _queuedforces.Add(force);
            }
        }

        public Force TotalForce { get; private set; }
        public float TotalMoment { get; private set; }
        
        protected IList<Force> Forces { get; private set; }

        public void Teleport(Vector2 destination)
        {
            Position = destination;
        }

        public void Update(GameTime gameTime)
        {
            UpdateInternal(gameTime);
        }

        protected void ResolveForces()
        {
            Forces.Clear();
            
            TotalForce = new Force();
            TotalMoment = 0f;

            foreach (var force in _queuedforces)
            {
                Forces.Add(force);
                TotalForce.AddVector(force.Vector);
                TotalMoment += CalculateMoment(force);
            }

            _queuedforces.Clear();
        }

        private float CalculateMoment(Force force)
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

        protected abstract void UpdateInternal(GameTime gameTime);

        public abstract void Draw();
    }
}
