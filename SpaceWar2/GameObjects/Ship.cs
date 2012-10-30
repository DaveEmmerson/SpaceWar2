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
        private readonly GraphicsDeviceManager _graphics;
        public IShipController Controller { private get; set; }

        private readonly IList<Arrow> _arrows;
        private readonly ThrusterArray _thrusterArray;
        private readonly EnergyStore _energyStore;
        private readonly Shield _shield;
        
        public Ship(string name, GraphicsDeviceManager graphics, Vector2 position, float radius, Color color)
            : base (position, radius, 1)
        {
            _graphics = graphics;
            _arrows = new List<Arrow>();
            Color = color;
            Name = name;
            Controller = new NullShipController();

            Armour = 100F;
            _energyStore = new EnergyStore(100F, 0.1F);
            _shield = new Shield(this, 100F, 0.1F);
            _thrusterArray = new ThrusterArray(this);
        }

        //Settings
        public string Name { get; protected set; }
        public bool ShowArrows { get; set; }

        public float Energy { get { return _energyStore.Level; } }
        public float Shields { get { return _shield.Level; } }

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
                    Expired = true;
                }
            }
        }

        protected override void UpdateInternal(GameTime gameTime)
        {
            var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _shield.Recharge(deltaT);
            _energyStore.Recharge(deltaT);

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
            return _energyStore.RequestEnergy(energyRequest);
        }
    }
}