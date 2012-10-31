﻿using DEMW.SpaceWar2.Controls;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    public class ThrusterArray
    {
        public const float ThrustPower = 50F;
        public const float ThrustEnergyCost = 0.1F;
        private const float RotationSpeed = 2F;

        private readonly IShip _ship;

        private float _angularVelocityTarget;

        private readonly Thruster _frontLeftThruster;
        private readonly Thruster _frontRightThruster;
        private readonly Thruster _backLeftThruster;
        private readonly Thruster _backRightThruster;

        public ThrusterArray(IShip ship)
        {
            _ship = ship;

            var forwardThrust = new Vector2(0, -ThrustPower);
            var backthrust = new Vector2(0, ThrustPower);

            var leftOfShip = new Vector2(-ship.Radius, 0);
            var rightOfShip = new Vector2(ship.Radius, 0);

            _frontLeftThruster = new Thruster(leftOfShip, forwardThrust, ThrustEnergyCost);
            _frontRightThruster = new Thruster(rightOfShip, forwardThrust, ThrustEnergyCost);
            _backLeftThruster = new Thruster(leftOfShip, backthrust, ThrustEnergyCost);
            _backRightThruster = new Thruster(rightOfShip, backthrust, ThrustEnergyCost);
        }

        public void CalculateThrustPattern(ShipAction action)
        {
            ResetThrusters();

            CalculateTargetAngularVelocity(action);
            AchieveTargetAngularVelocity();

            GenerateLinearThrust(action);
        }

        private void ResetThrusters()
        {
            _frontLeftThruster.Throttle = 0f;
            _frontRightThruster.Throttle = 0f;
            _backLeftThruster.Throttle = 0f;
            _backRightThruster.Throttle = 0f;
        }

        private void CalculateTargetAngularVelocity(ShipAction action)
        {
            _angularVelocityTarget = 0;
            if (action.HasFlag(ShipAction.TurnLeft))
            {
                _angularVelocityTarget -= RotationSpeed;
            }
            if (action.HasFlag(ShipAction.TurnRight))
            {
                _angularVelocityTarget += RotationSpeed;
            }
        }

        //TODO MW make the amonut of thrust proportional to how much thrust is needed.
        private void AchieveTargetAngularVelocity()
        {
            if (_ship.AngularVelocity < _angularVelocityTarget - 0.1f)
            {
                _frontLeftThruster.Throttle = 0.25f;
                _backRightThruster.Throttle = 0.25f;
            }
            else if (_ship.AngularVelocity > _angularVelocityTarget + 0.1f)
            {
                _frontRightThruster.Throttle = 0.25f;
                _backLeftThruster.Throttle = 0.25f;
            }
            else
            {
                _ship.AngularVelocity = _angularVelocityTarget;
            }
        }

        private void GenerateLinearThrust(ShipAction action)
        {
            float linearTarget = 0f;
            if (action.HasFlag(ShipAction.Thrust))
            {
                linearTarget += 1f;
            }

            if (action.HasFlag(ShipAction.ReverseThrust))
            {
                linearTarget -= 1f;
            }

            if (linearTarget > 0f)
            {
                _frontLeftThruster.Throttle += linearTarget;
                _frontRightThruster.Throttle += linearTarget;
            }
            else if (linearTarget < 0f)
            {
                _backLeftThruster.Throttle += 0f - linearTarget;
                _backRightThruster.Throttle += 0f - linearTarget;
            }
        }

        public void EngageThrusters()
        {
            var energyRequired = _frontRightThruster.EnergyRequired
                                 + _frontLeftThruster.EnergyRequired
                                 + _backLeftThruster.EnergyRequired
                                 + _backRightThruster.EnergyRequired;

            if (energyRequired <= 0f)
            {
                return;
            }

            var availableEnergy = _ship.RequestEnergy(energyRequired);

            var energyScalingFactor = availableEnergy/energyRequired;

            ApplyForce(_frontLeftThruster, energyScalingFactor);
            ApplyForce(_frontRightThruster, energyScalingFactor);
            ApplyForce(_backLeftThruster, energyScalingFactor);
            ApplyForce(_backRightThruster, energyScalingFactor);
        }

        private void ApplyForce(Thruster thruster, float energyScalingFactor)
        {
            var force = thruster.Engage(energyScalingFactor);
            if (force.Vector.Length() > 0)
            {
                _ship.ApplyInternalForce(force);
            }
        }
    }
}
