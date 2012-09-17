using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWar2
{
    class Ship : Circle
    {

        const float THRUST_POWER = 100F;
        const float THRUST_ENERGY_COST = 0.1F;
        const float ROTATION_SPEED = 180F;
        const float MAX_SHIELD_LEVEL = 100F;
        const float MAX_ENERGY_LEVEL = 100F;

        private float shields = MAX_SHIELD_LEVEL;
        public float Shields {

            get
            {

                return shields;

            }

            set
            {

                if (value > MAX_SHIELD_LEVEL)
                {

                    shields = MAX_SHIELD_LEVEL;

                }
                else
                {


                    shields = value;

                    if (shields < 0)
                    {

                        Armour += shields;
                        shields = 0;

                    }

                }

            }
        }

        public float ShieldRechargeRate { get; set; }

        private float energy;
        public float Energy {

            get
            {

                return energy;

            }

            set
            {

                if (value > MAX_ENERGY_LEVEL)
                {

                    energy = MAX_ENERGY_LEVEL;

                }
                else
                {

                    energy = value;

                }

            }
        }

        public float EnergyRechargeRate { get; set; }

        private float armour = 100;
        public float Armour
        {

            get
            {

                return armour;

            }

            set
            {

                if (armour < 0 && value > armour)
                {

                    throw new ArgumentException("Ship is already exploding. Can't repair.");

                }

                armour = value;

                if (armour < 0)
                {

                    exploding = true;
                    explosionTargetRadius = Radius * EXPLOSION_RADIUS_MULTIPLIER;
                    explosionRadiusIncrement = (EXPLOSION_RADIUS_MULTIPLIER - 1) * Radius / EXPLOSION_SPEED;

                }

            }

        }
    
        public string Name { get; set; }

        public float Rotation { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 Acceleration { get; set; }

        public IShipController Controller { get; set; }

        public bool DrawArrows { get; set; }

        private VertexPositionColor[] accelerationArrow;
        private VertexPositionColor[] velocityArrow;
        private VertexPositionColor[] rotationArrow;

        private const float EXPLOSION_RADIUS_MULTIPLIER = 1.4F;
        private const float EXPLOSION_SPEED = 0.5F;
        private bool exploding;
        private bool imploding;
        private float explosionTargetRadius;
        private float explosionRadiusIncrement;

        public Ship(string name, GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount)
             : base(graphics,position,radius,lineColor,lineCount)
        {

            Name = name;

            ShieldRechargeRate = 0.1F;

            EnergyRechargeRate = 0.01F;

            Energy = 100F;

            CreateVertices();

        }

        private void CreateVertices()
        {

            if (DrawArrows)
            {

                accelerationArrow = getArrow(Acceleration, Color.LimeGreen);

                velocityArrow = getArrow(Velocity, Color.Linen);

                rotationArrow = getArrow(new Vector2((float)Math.Sin(Rotation), -(float)Math.Cos(Rotation)), Color.Red);

            }

        }

        private VertexPositionColor[] getArrow(Vector2 vector, Color color)
        {

            VertexPositionColor[] arrow = new VertexPositionColor[5];

            if (vector == Vector2.Zero)

                return arrow;

            vector.Normalize();

            Vector2 perpendicular = new Vector2(-vector.Y, vector.X);

            float arrowSize = Radius / 4;
            
            

            arrow[0].Position = new Vector3(Position + vector * Radius, 0);
            arrow[0].Color = color;

            arrow[1].Position = new Vector3(Position + vector * Radius * 2, 0);
            arrow[1].Color = color;

            arrow[2].Position = new Vector3(Position + vector * (Radius * 2 - arrowSize) -
                                            perpendicular * arrowSize, 0);

            arrow[2].Color = color;

            arrow[3].Position = new Vector3(Position + vector * (Radius * 2 - arrowSize) -
                                            -perpendicular * arrowSize, 0);
            arrow[3].Color = color;

            arrow[4].Position = new Vector3(Position + vector * Radius * 2, 0);
            arrow[4].Color = color;
            
            
            return arrow;

        }

        public override void Draw()
        {
            CreateVertices();

            if (DrawArrows)
            {

                DrawLineStrip(accelerationArrow);
                DrawLineStrip(velocityArrow);
                DrawLineStrip(rotationArrow);


            }

            base.Draw();
        }

        public void Update(GameTime gameTime)
        {
            
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (exploding)
            {

                if (Radius > explosionTargetRadius)
                {

                    exploding = false;
                    imploding = true;

                }

                Radius = Radius + explosionRadiusIncrement * deltaT;
            }

            else if (imploding)
            {

                Radius = Radius - explosionRadiusIncrement * (deltaT + 1 / Radius);

                if (Radius <= 0)
                {

                    imploding = false;
                    Expired = true;

                }

            }
            else
            {

                if (Shields < MAX_SHIELD_LEVEL)
                {

                    float increaseRequired = Math.Min(MAX_SHIELD_LEVEL - Shields,ShieldRechargeRate);

                    

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

                   energyToThrust();

                }

                if (action.HasFlag(ShipAction.ReverseThrust))
                {

                    energyToThrust(reverse: true);

                }

                if (action.HasFlag(ShipAction.TurnLeft))
                {

                    Rotation -= (float)Math.PI / ROTATION_SPEED;

                }

                if (action.HasFlag(ShipAction.TurnRight))
                {

                    Rotation += (float)Math.PI / ROTATION_SPEED;

                }

            }

            Velocity += Acceleration * deltaT;

            Position = Position + Velocity * deltaT;

        }

        public void Damage(int amount)
        {

            Shields -= amount;

        }

        private void energyToThrust(bool reverse = false)
        {

            float thrustPower = THRUST_POWER;

            if (Energy < THRUST_ENERGY_COST)
            {

                thrustPower = thrustPower / THRUST_ENERGY_COST * energy;
                Energy = 0;

            }
            else
            {

                Energy -= THRUST_ENERGY_COST;

            }

            if (thrustPower > 0)
            {
                if (reverse)
                {

                    thrustPower *= -1;

                }

                Acceleration += new Vector2(thrustPower * (float)Math.Sin(Rotation),
                                       -thrustPower * (float)Math.Cos(Rotation));

            }


        }

    }
}
