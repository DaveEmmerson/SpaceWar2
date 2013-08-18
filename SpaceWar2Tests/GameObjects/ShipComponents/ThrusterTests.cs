using DEMW.SpaceWar2.Core.GameObjects.ShipComponents;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace DEMW.SpaceWar2Tests.GameObjects.ShipComponents
{
    [TestFixture]
    class ThrusterTests
    {
        private Thruster _thruster;
        private Vector2 _position;
        private Vector2 _direction;

        [SetUp]
        public void Setup()
        {
            _position = new Vector2(1, 1);
            _direction = new Vector2(0, -1);

            _thruster = new Thruster(_position, _direction, 0.1f);
        }

        [Test]
        public void Thruster_maxEnergyDraw_cannot_be_zero()
        {
            var exception = Assert.Throws<ArgumentException>(() => _thruster = new Thruster(_position, _direction, 0f));
            Assert.AreEqual("Must be positive and non-zero.\r\nParameter name: maxEnergyDraw", exception.Message);
        }

        [Test]
        public void Thruster_maxEnergyDraw_cannot_be_negative()
        {
            var exception = Assert.Throws<ArgumentException>(() => _thruster = new Thruster(_position, _direction, -0.1f));
            Assert.AreEqual("Must be positive and non-zero.\r\nParameter name: maxEnergyDraw", exception.Message);
        }

        [Test]
        public void Throttle_can_be_set_to_a_value_between_zero_and_one()
        {
            _thruster.Throttle = 0.4f;
            Assert.AreEqual(0.4f, _thruster.Throttle);
        }

        [Test]
        public void Throttle_is_zero_if_it_is_set_below_zero()
        {
            _thruster.Throttle = -0.4f;
            Assert.AreEqual(0f, _thruster.Throttle);
        }

        [Test]
        public void Throttle_is_one_if_it_is_set_above_one()
        {
            _thruster.Throttle = 1.4f;
            Assert.AreEqual(1f, _thruster.Throttle);
        }

        [Test]
        public void EnergyRequired_is_Throttle_times_max_Energy_draw()
        {
            _thruster.Throttle = 0.4f;
            Assert.AreEqual(0.4f * 0.1f, _thruster.EnergyRequired);
        }

        [Test]
        public void Engage_produces_no_force_when_no_Throttle_and_full_energyScalingFactor()
        {
            const float energyScalingFactor = 1f;
            _thruster.Throttle = 0f;

            var force = _thruster.Engage(energyScalingFactor);
            Assert.AreEqual(_position, force.Displacement);
            Assert.AreEqual(Vector2.Zero, force.Vector);
        }

        [Test]
        public void Engage_produces_no_force_when_some_Throttle_and_zero_energyScalingFactor()
        {
            const float energyScalingFactor = 0f;
            _thruster.Throttle = 0.8f;

            var force = _thruster.Engage(energyScalingFactor);
            Assert.AreEqual(_position, force.Displacement);
            Assert.AreEqual(Vector2.Zero, force.Vector);
        }

        [Test]
        public void Engage_produces_no_force_when_some_Throttle_and_negative_energyScalingFactor()
        {
            const float energyScalingFactor = -0.2f;
            _thruster.Throttle = 0.8f;

            var force = _thruster.Engage(energyScalingFactor);
            Assert.AreEqual(_position, force.Displacement);
            Assert.AreEqual(Vector2.Zero, force.Vector);
        }

        [Test]
        public void Engage_produces_full_force_when_full_Throttle_and_full_energyScalingFactor()
        {
            const float energyScalingFactor = 1f;
            _thruster.Throttle = 1f;

            var force = _thruster.Engage(energyScalingFactor);
            Assert.AreEqual(_position, force.Displacement);
            Assert.AreEqual(_direction, force.Vector);
        }

        [Test]
        public void Engage_produces_force_proportional_to_Throttle_time_by_energyScalingFactor()
        {
            const float energyScalingFactor = 0.8f;
            _thruster.Throttle = 0.5f;

            var force = _thruster.Engage(energyScalingFactor);
            Assert.AreEqual(_position, force.Displacement);
            Assert.AreEqual(_direction * energyScalingFactor * _thruster.Throttle, force.Vector);
        }

        [Test]
        public void Engage_caps_energyScalingFactor_to_one()
        {
            const float energyScalingFactor = 1.5f;
            const float expectedCappedEnergyScalingFactor = 1f;
            _thruster.Throttle = 0.5f;

            var force = _thruster.Engage(energyScalingFactor);
            Assert.AreEqual(_position, force.Displacement);
            Assert.AreEqual(_direction * expectedCappedEnergyScalingFactor * _thruster.Throttle, force.Vector);
        }

    }
}
