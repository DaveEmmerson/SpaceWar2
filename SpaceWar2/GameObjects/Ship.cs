using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Utils.XnaWrappers;
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
        private readonly IList<IArrow> _arrows;

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
            _arrows = new List<IArrow>();
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

            ShipAction action = Controller.Action;
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

        public float RequestEnergy(float amountRequested)
        {
            return _energyStore.RequestEnergy(amountRequested);
        }

        public override void Draw(IGraphicsDevice graphicsDevice = null)
        {
            DrawArrows(graphicsDevice);
        }

        private void DrawArrows(IGraphicsDevice graphicsDevice)
        {
            _arrows.Clear();
            
            if (!ShowArrows)
            {
                return;
            }

            var accelerationArrow = _graphicsFactory.CreateAccelerationArrow(TotalForce, Radius);
            var velocityArrow = _graphicsFactory.CreateVelocityArrow(Velocity, Radius);
            var rotationArrow = _graphicsFactory.CreateRotationArrow(Rotation, Radius);

            _arrows.Add(accelerationArrow);
            _arrows.Add(velocityArrow);
            _arrows.Add(rotationArrow);

            foreach (var arrow in Forces.Select(force => _graphicsFactory.CreateForceArrow(force, Radius)))
            {
                _arrows.Add(arrow);
            }

            foreach (var arrow in _arrows)
            {
                arrow.Draw(graphicsDevice);
            }
        }

        public string DebugDetails
        {
            get
            {
                var stringBuilder = new StringBuilder();

                AppendLine(stringBuilder, "{0}.Position.X: {1}", Position.X);
                AppendLine(stringBuilder, "{0}.Position.Y: {1}", Position.Y);
                AppendLine(stringBuilder, "{0}.Velocity.X: {1}", Velocity.X);
                AppendLine(stringBuilder, "{0}.Velocity.Y: {1}", Velocity.Y);
                stringBuilder.AppendLine();
                AppendLine(stringBuilder, "{0}.Rotation: {1}", Rotation);
                AppendLine(stringBuilder, "{0}.AngularVelocity: {1}", AngularVelocity);
                stringBuilder.AppendLine();
                AppendLine(stringBuilder, "{0}.Shields: {1}", Shields);
                AppendLine(stringBuilder, "{0}.Armour: {1}", Armour);
                AppendLine(stringBuilder, "{0}.Energy: {1}", Energy);

                return stringBuilder.ToString();
            }
        }

        private void AppendLine(StringBuilder stringBuilder, string formatString, float value)
        {
            stringBuilder.AppendFormat(formatString, Name, value.ToString(CultureInfo.InvariantCulture));
            stringBuilder.AppendLine();
        }
    }
}