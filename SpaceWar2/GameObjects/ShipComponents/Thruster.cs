using System;
using DEMW.SpaceWar2.Core.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    internal class Thruster
    {
        private readonly Vector2 _direction;
        private readonly Vector2 _position;
        private readonly float _maxEnergyDraw;

        internal Thruster(Vector2 position, Vector2 direction, float maxEnergyDraw)
        {
            _position = position;
            _direction = direction;
            if (maxEnergyDraw <= 0)
            {
                throw new ArgumentException("Must be positive and non-zero.", "maxEnergyDraw");
            }
            _maxEnergyDraw = maxEnergyDraw;
        }

        internal float EnergyRequired { get { return Throttle * _maxEnergyDraw; } }

        private float _throttle;
        internal float Throttle
        {
            get { return _throttle; }
            set { _throttle = value < 0f ? 0f : value > 1f ? 1f : value; }
        }

        internal Force Engage(float energyScalingFactor)
        {
            if (energyScalingFactor <= 0f)
            {
                return new Force(Vector2.Zero, _position);
            }

            if (energyScalingFactor > 1f)
            {
                energyScalingFactor = 1f;
            }

            var thrust = energyScalingFactor * Throttle * _direction;
            return new Force(thrust, _position);
        }
    }
}
