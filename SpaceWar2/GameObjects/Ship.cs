using System;
using System.Collections.Generic;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using DEMW.SpaceWar2.Graphics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{
    public class Ship : GameObject, IShip
    {
        private const float MaxEnergyLevel = 100F;
        private const float ExplosionRadiusMultiplier = 1.4F;
        private const float ExplosionSpeed = 0.5F;
        private const float EnergyRechargeRate = 0.01F;

        private readonly GraphicsDeviceManager _graphics;
        public IShipController Controller { private get; set; }

        private readonly IList<Arrow> _arrows;
        private readonly ThrusterArray _thrusterArray;
        private readonly Shield _shield;
        
        public Ship(string name, GraphicsDeviceManager graphics, Vector2 position, float radius, Color color)
            : base (position, radius, 1)
        {
            _graphics = graphics;
            _arrows = new List<Arrow>();
            Color = color;
            Name = name;
            Controller = new NullShipController();

            Energy = 100F;
            Armour = 100F;
            _shield = new Shield(this, 100F, 0.1F);

            _thrusterArray = new ThrusterArray(this);
        }

        //Settings
        public string Name { get; protected set; }
        public bool ShowArrows { get; set; }

        public float Shields
        {
            get { return _shield.Level; }
        }

        private float _energy;
        public float Energy
        {
            get { return _energy; }
            set { _energy = value > MaxEnergyLevel ? MaxEnergyLevel : value; }
        }

        private float _armour;
        public float Armour
        {
            get { return _armour; }

            set
            {
                if (_armour < 0 && value > _armour)
                {
                    throw new ArgumentException("Ship is already exploding. Can't repair.");
                }

                _armour = value;

                if (_armour < 0)
                {
                    _exploding = true;
                    _explosionTargetRadius = Radius * ExplosionRadiusMultiplier;
                    _explosionRadiusIncrement = (ExplosionRadiusMultiplier - 1) * Radius / ExplosionSpeed;
                }
            }
        }

        private bool _exploding;
        private bool _imploding;
        private float _explosionTargetRadius;
        private float _explosionRadiusIncrement;

        protected override void UpdateInternal(GameTime gameTime)
        {
            var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_exploding)
            {
                if (Radius > _explosionTargetRadius)
                {
                    _exploding = false;
                    _imploding = true;
                }

                Radius = Radius + _explosionRadiusIncrement * deltaT;
            }

            else if (_imploding)
            {
                Radius = Radius - _explosionRadiusIncrement * (deltaT + 1 / Radius);

                if (Radius <= 0)
                {
                    _imploding = false;
                    Expired = true;
                }
            }
            else
            {
                _shield.Recharge(deltaT);
                
                Energy += EnergyRechargeRate;
            }

            ShipAction action = Controller.GetAction();
            _thrusterArray.CalculateThrustPattern(action);
            _thrusterArray.EngageThrusters();

            ResolveForces();
            var acceleration = (TotalForce.Vector / Mass);
            Velocity += acceleration * deltaT;
            Position += Velocity * deltaT;

            var angularAcceleration = (TotalMoment / MomentOfInertia);
            AngularVelocity += angularAcceleration * deltaT;
            Rotation += AngularVelocity * deltaT;
        }

        public override void Draw()
        {
            DrawArrows();
        }

        private void DrawArrows()
        {
            _arrows.Clear();
            if (ShowArrows)
            {
                var accelerationArrow = new Arrow(_graphics, TotalForce.Displacement, TotalForce.Vector, Color.LimeGreen, Radius);
                var velocityArrow = new Arrow(_graphics, Vector2.Zero, Velocity, Color.Linen, Radius);
                var rotationAngle = new Vector2((float)Math.Sin(Rotation) * Radius * 2, -(float)Math.Cos(Rotation) * Radius * 2);
                var rotationArrow = new Arrow(_graphics, Vector2.Zero, rotationAngle, Color.Red, Radius);

                _arrows.Add(accelerationArrow);
                _arrows.Add(velocityArrow);
                _arrows.Add(rotationArrow);

                foreach (var force in Forces)
                {
                    var arrow = new Arrow(_graphics, force.Displacement, force.Vector, Color.Yellow, Radius);
                    _arrows.Add(arrow);
                }
            }

            foreach (var arrow in _arrows)
            {
                arrow.Draw();
            }
        }

        public void Damage(int amount)
        {
            var damageRemaining = _shield.Damage(amount);

            Armour -= damageRemaining;
        }

        public float RequestEnergy(float energyRequest)
        {
            float energyDepletion = Energy < energyRequest ? Energy : energyRequest;

            Energy -= energyDepletion;
            return energyDepletion;
        }
    }
}