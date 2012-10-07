using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceWar2
{
    class Ship : GameObject
    {
        private const float ThrustPower = 100F;
        private const float ThrustEnergyCost = 0.1F;
        private const float RotationSpeed = 180F;
        private const float MaxShieldLevel = 100F;
        private const float MaxEnergyLevel = 100F;
        private const float ExplosionRadiusMultiplier = 1.4F;
        private const float ExplosionSpeed = 0.5F;
        private const float ShieldRechargeRate = 0.1F;
        private const float EnergyRechargeRate = 0.01F;

        private readonly GraphicsDeviceManager _graphics;
        public IShipController Controller { private get; set; }

        private readonly Circle _model;
        private readonly IList<Arrow> _arrows;
        
        public Ship(string name, GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount)
            : base (position, radius, 1)
        {
            _graphics = graphics;
            _model = new Circle(graphics, radius, lineColor, lineCount);
            _arrows = new List<Arrow>();

            Name = name;

            Energy = 100F;
            Armour = 100F;
        }

        //Settings
        public string Name { get; set; }
        public bool ShowArrows { get; set; }
        
        //State
        public Vector2 Velocity { private get; set; }
        private float _rotation;

        private float _shields = MaxShieldLevel;
        public float Shields
        {
            get { return _shields; }

            set
            {
                if (value > MaxShieldLevel)
                {
                    _shields = MaxShieldLevel;
                }
                else
                {
                    _shields = value;

                    if (_shields < 0)
                    {
                        Armour += _shields;
                        _shields = 0;
                    }
                }
            }
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
        
        public override void Draw()
        {
            DrawArrows();
            _model.Draw();
        }

        private void DrawArrows()
        {
            _arrows.Clear();
            if (ShowArrows)
            {
                var accelerationArrow = new Arrow(_graphics, ResolveForces(), Color.LimeGreen, Radius);
                var velocityArrow = new Arrow(_graphics, Velocity, Color.Linen, Radius);
                var rotationAngle = new Vector2((float)Math.Sin(_rotation) * Radius * 2, -(float)Math.Cos(_rotation) * Radius * 2);
                var rotationArrow = new Arrow(_graphics, rotationAngle, Color.Red, Radius);

                _arrows.Add(accelerationArrow);
                _arrows.Add(velocityArrow);
                _arrows.Add(rotationArrow);

                foreach (var force in Forces)
                {
                    var arrow = new Arrow(_graphics, force, Color.Yellow, Radius);
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
            Shields -= amount;
        }

        public void Update(GameTime gameTime)
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
                if (Shields < MaxShieldLevel)
                {
                    float increaseRequired = Math.Min(MaxShieldLevel - Shields,ShieldRechargeRate);

                    if (Energy >= increaseRequired)
                    {
                        Shields += increaseRequired;
                        Energy -= increaseRequired;
                    }
                    else if (Energy > 0)
                    {
                        Shields += Energy;
                        Energy = 0;
                    }
                }

                Energy += EnergyRechargeRate;
            }

            if (Controller != null)
            {
                ShipAction action = Controller.GetAction();

                if (action.HasFlag(ShipAction.Thrust))
                {
                   EngageThrusters();
                }

                if (action.HasFlag(ShipAction.ReverseThrust))
                {
                    EngageThrusters(reverse: true);
                }

                if (action.HasFlag(ShipAction.TurnLeft))
                {
                    _rotation -= (float)Math.PI / RotationSpeed;
                }

                if (action.HasFlag(ShipAction.TurnRight))
                {
                    _rotation += (float)Math.PI / RotationSpeed;
                }
            }

            Velocity +=  ResolveForces() * deltaT;
            Position = Position + Velocity * deltaT;
        }

        private void EngageThrusters(bool reverse = false)
        {
            float thrustPower = ThrustPower;

            if (Energy < ThrustEnergyCost)
            {
                thrustPower = thrustPower / ThrustEnergyCost * _energy;
                Energy = 0;
            }
            else
            {
                Energy -= ThrustEnergyCost;
            }

            if (thrustPower > 0)
            {
                if (reverse)
                {
                    thrustPower *= -1;
                }
                
                var force = new Vector2(thrustPower*(float) Math.Sin(_rotation), -thrustPower*(float) Math.Cos(_rotation));
                ApplyForce(force);
            }
        }
    }
}