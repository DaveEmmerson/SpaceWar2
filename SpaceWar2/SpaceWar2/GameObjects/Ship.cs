using System;
using System.Collections.Generic;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
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
        private readonly Thruster _mainThruster;
        private readonly Thruster _reverseThruster;
        
        public Ship(string name, GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount)
            : base (position, radius, 1)
        {
            _graphics = graphics;
            _model = new Circle(graphics, radius, lineColor, lineCount);
            _arrows = new List<Arrow>();

            Name = name;

            Energy = 100F;
            Armour = 100F;

            _mainThruster = new Thruster(this, Vector2.Zero, new Vector2(0, 100), ThrustEnergyCost);
            _reverseThruster = new Thruster(this, Vector2.Zero, new Vector2(0, -100), ThrustEnergyCost);
        }

        //Settings
        public string Name { get; set; }
        public bool ShowArrows { get; set; }

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
                var rotationAngle = new Vector2((float)Math.Sin(Rotation) * Radius * 2, -(float)Math.Cos(Rotation) * Radius * 2);
                var rotationArrow = new Arrow(_graphics, rotationAngle, Color.Red, Radius);

                _arrows.Add(accelerationArrow);
                _arrows.Add(velocityArrow);
                _arrows.Add(rotationArrow);

                foreach (var force in Forces)
                {
                    var arrow = new Arrow(_graphics, force.Vector, Color.Yellow, Radius);
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
                   _mainThruster.Engage();
                }

                if (action.HasFlag(ShipAction.ReverseThrust))
                {
                    _reverseThruster.Engage();
                }

                if (action.HasFlag(ShipAction.TurnLeft))
                {
                    Rotation -= (float)Math.PI / RotationSpeed;
                }

                if (action.HasFlag(ShipAction.TurnRight))
                {
                    Rotation += (float)Math.PI / RotationSpeed;
                }
            }

            Velocity +=  ResolveForces() * deltaT;
            Position = Position + Velocity * deltaT;
        }

        public float RequestEnergy(float energyRequest)
        {
            float energyDepletion = Energy < energyRequest ? Energy : energyRequest;

            Energy -= energyDepletion;
            return energyDepletion;
        }
    }
}