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
        public IShipController Controller { private get; set; }

        private readonly IEnergyStore _energyStore;
        private readonly IShield _shield;
        private readonly IHull _hull;
        private readonly IThrusterArray _thrusterArray;

        private readonly IGraphicsFactory _graphicsFactory;
        private readonly IList<Arrow> _arrows;

        public Ship(string name, Vector2 position, float radius, Color color, IGraphicsFactory graphicsFactory, IShipComponentFactory shipComponentFactory)
            : base (position, radius, 1)
        {
            Name = name;
            Color = color;
            
            Controller = new NullShipController();

            _energyStore = shipComponentFactory.CreateEnergyStore();
            _shield = shipComponentFactory.CreateShield(this);
            _hull = shipComponentFactory.CreateHull(this);
            _thrusterArray = shipComponentFactory.CreateThrusterArray(this);

            _graphicsFactory = graphicsFactory;
            _arrows = new List<Arrow>();
        }

        public string Name { get; private set; }
        public bool ShowArrows { private get; set; }

        public float Energy { get { return _energyStore.Level; } }
        public float Shields { get { return _shield.Level; } }
        public float Armour { get { return _hull.Level; } }       

        protected override void UpdateInternal(float deltaT)
        {
            _shield.Recharge(deltaT);
            _energyStore.Recharge(deltaT);

            ShipAction action = Controller.GetAction();
            _thrusterArray.CalculateThrustPattern(action);
            _thrusterArray.EngageThrusters();
        }
        
        public void Damage(float amount)
        {
            var damageRemaining = _shield.Damage(amount);

            if (damageRemaining > 0F)
            {
                _hull.Damage(damageRemaining);

                if (_hull.Level <= 0F)
                {
                    Expired = true;
                }
            }
        }

        public float RequestEnergy(float energyRequest)
        {
            return _energyStore.RequestEnergy(energyRequest);
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
                var accelerationArrow = _graphicsFactory.CreateAccelerationArrow(TotalForce.Displacement, TotalForce.Vector, Radius);
                var velocityArrow = _graphicsFactory.CreateVelocityArrow(Vector2.Zero, Velocity, Radius);
                
                var rotationAngle = new Vector2((float)Math.Sin(Rotation) * Radius * 2, -(float)Math.Cos(Rotation) * Radius * 2);
                var rotationArrow = _graphicsFactory.CreateRotationArrow(Vector2.Zero, rotationAngle, Radius);

                _arrows.Add(accelerationArrow);
                _arrows.Add(velocityArrow);
                _arrows.Add(rotationArrow);

                foreach (var force in Forces)
                {
                    var arrow = _graphicsFactory.CreateForceArrow(force.Displacement, force.Vector, Radius);
                    _arrows.Add(arrow);
                }
            }

            foreach (var arrow in _arrows)
            {
                arrow.Draw();
            }
        }
    }
}