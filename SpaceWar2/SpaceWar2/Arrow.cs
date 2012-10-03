using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWar2
{
    class Arrow
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly VertexPositionColor[] _vertices;

        internal Arrow(GraphicsDeviceManager graphics, Vector2 vector, Color color, float radius)
        {
            _vertices = new VertexPositionColor[5];
            _graphics = graphics;

            if (vector == Vector2.Zero)
            {
                return;
            }

            vector.Normalize();

            var perpendicular = new Vector2(-vector.Y, vector.X);

            float arrowSize = radius / 4f;

            _vertices[0].Position = new Vector3(vector * radius, 0);
            _vertices[1].Position = new Vector3(vector * radius * 2, 0);
            _vertices[2].Position = new Vector3(vector * (radius * 2 - arrowSize) - perpendicular * arrowSize, 0);
            _vertices[3].Position = new Vector3(vector * (radius * 2 - arrowSize) - -perpendicular * arrowSize, 0);
            _vertices[4].Position = new Vector3(vector * radius * 2, 0);

            _vertices[0].Color = color;
            _vertices[1].Color = color;
            _vertices[2].Color = color;
            _vertices[3].Color = color;
            _vertices[4].Color = color;
        }

        public virtual void Draw()
        {
            DrawLineStrip(_vertices);
        }

        internal void DrawLineStrip(VertexPositionColor[] array)
        {
            _graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, array, 0, array.Length - 1);
        }

        //const float ThrustPower = 100F;
        //const float ThrustEnergyCost = 0.1F;
        //const float RotationSpeed = 180F;
        //const float MaxShieldLevel = 100F;
        //const float MaxEnergyLevel = 100F;

        //private float _shields = MaxShieldLevel;
        
        //public float Shields 
        //{
        //    get { return _shields; }

        //    set
        //    {
        //        if (value > MaxShieldLevel)
        //        {
        //            _shields = MaxShieldLevel;
        //        }
        //        else
        //        {
        //            _shields = value;

        //            if (_shields < 0)
        //            {
        //                Armour += _shields;
        //                _shields = 0;
        //            }
        //        }
        //    }
        //}

        //public float ShieldRechargeRate { get; set; }

        //private float _energy;
        //public float Energy 
        //{
        //    get { return _energy; }
        //    set { _energy = value > MaxEnergyLevel ? MaxEnergyLevel : value; }
        //}

        //public float EnergyRechargeRate { get; set; }

        //private float _armour = 100;
        //public float Armour
        //{
        //    get { return _armour; }

        //    set
        //    {
        //        if (_armour < 0 && value > _armour)
        //        {
        //            throw new ArgumentException("Ship is already exploding. Can't repair.");
        //        }

        //        _armour = value;

        //        if (_armour < 0)
        //        {
        //            _exploding = true;
        //            _explosionTargetRadius = radius * ExplosionRadiusMultiplier;
        //            _explosionRadiusIncrement = (ExplosionRadiusMultiplier - 1) * radius / ExplosionSpeed;
        //        }
        //    }
        //}
    
        //public string Name { get; set; }

        //public float Rotation { get; set; }

        //public Vector2 Velocity { get; set; }
        
        //public IShipController Controller { get; set; }

        //public bool DrawArrows { get; set; }

        //private VertexPositionColor[] _accelerationArrow;
        //private VertexPositionColor[] _velocityArrow;
        //private VertexPositionColor[] _rotationArrow;

        //private const float ExplosionRadiusMultiplier = 1.4F;
        //private const float ExplosionSpeed = 0.5F;
        //private bool _exploding;
        //private bool _imploding;
        //private float _explosionTargetRadius;
        //private float _explosionRadiusIncrement;

        //public Ship(string name, GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount)
        //{
        //    Name = name;
        //    ShieldRechargeRate = 0.1F;
        //    EnergyRechargeRate = 0.01F;
        //    Energy = 100F;
        //    CreateVertices();

        //    Position = position;
        //    radius = radius;
            
        //    Model = new Circle(graphics, radius, lineColor, lineCount);
        //}

        //private Circle Model { get; set; }

        //private void CreateVertices()
        //{
        //    if (DrawArrows)
        //    {
        //        _accelerationArrow = GetArrow(Acceleration, Color.LimeGreen);
        //        _velocityArrow = GetArrow(Velocity, Color.Linen);
        //        _rotationArrow = GetArrow(new Vector2((float)Math.Sin(Rotation), -(float)Math.Cos(Rotation)), Color.Red);
        //    }
        //}
        
        //public bool Expired { get; private set; }
        //public Vector2 Position { get; set; }
        //public Vector2 Acceleration { get; set; }
        //public float Mass { get; set; }
        //public float radius { get; set; }

        //public void Draw()
        //{
        //    CreateVertices();

        //    if (DrawArrows)
        //    {
        //        Model.DrawLineStrip(_accelerationArrow);
        //        Model.DrawLineStrip(_velocityArrow);
        //        Model.DrawLineStrip(_rotationArrow);
        //    }

        //    Model.Draw();
        //}

        //public void Update(GameTime gameTime)
        //{
        //    var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

        //    if (_exploding)
        //    {
        //        if (radius > _explosionTargetRadius)
        //        {
        //            _exploding = false;
        //            _imploding = true;
        //        }

        //        radius = radius + _explosionRadiusIncrement * deltaT;
        //    }

        //    else if (_imploding)
        //    {
        //        radius = radius - _explosionRadiusIncrement * (deltaT + 1 / radius);

        //        if (radius <= 0)
        //        {
        //            _imploding = false;
        //            Expired = true;
        //        }
        //    }
        //    else
        //    {
        //        if (Shields < MaxShieldLevel)
        //        {
        //            float increaseRequired = Math.Min(MaxShieldLevel - Shields,ShieldRechargeRate);

        //            if (Energy >= increaseRequired)
        //            {
        //                Shields += increaseRequired;
        //                Energy -= increaseRequired;
        //            }
        //            else if (Energy > 0)
        //            {
        //                Shields += Energy;
        //                Energy = 0;
        //            }
        //        }

        //        Energy += EnergyRechargeRate;
        //    }

        //    if (Controller != null)
        //    {
        //        ShipAction action = Controller.GetAction();

        //        if (action.HasFlag(ShipAction.Thrust))
        //        {
        //           EnergyToThrust();
        //        }

        //        if (action.HasFlag(ShipAction.ReverseThrust))
        //        {
        //            EnergyToThrust(reverse: true);
        //        }

        //        if (action.HasFlag(ShipAction.TurnLeft))
        //        {
        //            Rotation -= (float)Math.PI / RotationSpeed;
        //        }

        //        if (action.HasFlag(ShipAction.TurnRight))
        //        {
        //            Rotation += (float)Math.PI / RotationSpeed;
        //        }
        //    }

        //    Velocity += Acceleration * deltaT;

        //    Position = Position + Velocity * deltaT;
        //}

        //public void Damage(int amount)
        //{
        //    Shields -= amount;
        //}

        //private void EnergyToThrust(bool reverse = false)
        //{
        //    float thrustPower = ThrustPower;

        //    if (Energy < ThrustEnergyCost)
        //    {
        //        thrustPower = thrustPower / ThrustEnergyCost * _energy;
        //        Energy = 0;
        //    }
        //    else
        //    {
        //        Energy -= ThrustEnergyCost;
        //    }

        //    if (thrustPower > 0)
        //    {
        //        if (reverse)
        //        {
        //            thrustPower *= -1;
        //        }

        //        Acceleration += new Vector2(thrustPower * (float)Math.Sin(Rotation),
        //                               -thrustPower * (float)Math.Cos(Rotation));
        //    }
        //}
    }
}
