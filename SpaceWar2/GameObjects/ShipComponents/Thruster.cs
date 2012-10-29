﻿using System;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    public class Thruster
    {
        private readonly Vector2 _direction;
        private readonly Vector2 _position;
        private readonly float _maxEnergyDraw;

        public Thruster(Vector2 position, Vector2 direction, float maxEnergyDraw)
        {
            _position = position;
            _direction = direction;
            if (maxEnergyDraw <= 0)
            {
                throw new ArgumentException("Must be positive and non-zero.", "maxEnergyDraw");
            }
            _maxEnergyDraw = maxEnergyDraw;
        }

        public float EnergyRequired { get { return Throttle * _maxEnergyDraw; } }

        private float _throttle;
        public float Throttle
        {
            get { return _throttle; }
            set { _throttle = value < 0f ? 0f : value > 1f ? 1f : value; }
        }

        public Force Engage(float energyScalingFactor)
        {
            if (energyScalingFactor <= 0f)
            {
                return new Force(Vector2.Zero, _position);
            }

            if (energyScalingFactor > 1f)
            {
                energyScalingFactor = 1f;
            }

            Vector2 thrust = energyScalingFactor * Throttle * _direction;
            return new Force(thrust, _position);
        }
    }
}
