using System;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.GameObjects.ShipComponents;
using DEMW.SpaceWar2.GameObjects;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects.ShipComponents
{
    [TestFixture]
    class ShieldTests
    {
        private IShip _ship;
        private Shield _shield;
        private const float shieldStartLevel = 100F;
        private const float shieldRechargeRate = 1F;

        [SetUp]
        public void SetUp()
        {
            _ship = Substitute.For<IShip>();
            _ship.RequestEnergy(Arg.Any<float>()).Returns(x => x[0]);

            _shield = new Shield(_ship, shieldStartLevel, shieldRechargeRate);
        }

        [Test]
        public void Damage_reduces_shieldLevel_by_DamageAmount_and_returns_zero_when_DamageAmound_is_less_than_shieldLevel()
        {
            const float damage = shieldStartLevel / 10F;

            var damageRemaining = _shield.Damage(damage);

            Assert.AreEqual(shieldStartLevel - damage, _shield.Level);
            Assert.AreEqual(0F, damageRemaining);
        }

        [Test]
        public void Damage_does_not_reduce_shieldLevel_when_DamageAmount_is_zero()
        {
            var damageRemaining = _shield.Damage(0f);

            Assert.AreEqual(shieldStartLevel, _shield.Level);
            Assert.AreEqual(0F, damageRemaining);
        }

        [Test]
        public void Damage_does_not_affect_shieldLevel_when_DamageAmount_is_negative_and_an_exception_is_thrown()
        {
            const float negativeDamage = -10f;
            
            var exception = Assert.Throws<ArgumentException>(()=> _shield.Damage(negativeDamage));

            Assert.AreEqual("Must not be negative.\r\nParameter name: amount", exception.Message);
            Assert.AreEqual(shieldStartLevel, _shield.Level);
        }

        [Test]
        public void Damage_reduces_ShieldLevel_to_zero_and_returns_excess_when_DamageAmound_is_greater_than_SheildLevel()
        {
            const float excessDamage = 50F;
            const float damage = shieldStartLevel + excessDamage;

            var damageRemaining = _shield.Damage(damage);

            Assert.AreEqual(0, _shield.Level);
            Assert.AreEqual(excessDamage, damageRemaining);
        }

        [Test]
        public void Recharge_on_fully_charged_shield_does_not_call_RequestEnergy_on_ship()
        {
            const float deltaT = shieldStartLevel / shieldRechargeRate / 10F;
            
            _shield.Recharge(deltaT);

            Assert.AreEqual(shieldStartLevel, _shield.Level);
            _ship.DidNotReceive().RequestEnergy(Arg.Any<float>());
        }

        [Test]
        public void Recharge_requests_energy_from_ship_and_increases_SheildLevel_by_factor_of_deltaT_and_RechargeRate()
        {
            const float damage = shieldStartLevel / 2F;
            const float deltaT = shieldStartLevel / shieldRechargeRate / 10F;
            const float expectedIncrease = deltaT * shieldRechargeRate;

            _shield.Damage(damage);
            _shield.Recharge(deltaT);
            
            Assert.AreEqual(shieldStartLevel - damage + expectedIncrease, _shield.Level);
            _ship.Received().RequestEnergy(expectedIncrease);
        }

        [Test]
        public void Recharge_only_requests_what_it_needs_to_reach_maxium_ShieldLevel()
        {
            const float damage = shieldStartLevel / 2F;
            const float deltaT = shieldStartLevel / shieldRechargeRate * 2F;

            _shield.Damage(damage);
            _shield.Recharge(deltaT);

            Assert.AreEqual(shieldStartLevel, _shield.Level);
            _ship.Received().RequestEnergy(damage);
        }

        [Test]
        public void Recharge_has_no_effect_if_no_energy_available()
        {
            const float damage = shieldStartLevel / 2F;
            const float deltaT = shieldStartLevel / shieldRechargeRate;

            _ship.RequestEnergy(Arg.Any<float>()).Returns(0F);

            _shield.Damage(damage);

            _shield.Recharge(deltaT);

            Assert.AreEqual(shieldStartLevel - damage, _shield.Level);
        }

        [Test]
        public void Recharge_only_recharges_by_available_energy()
        {
            const float damage = shieldStartLevel / 2F;
            const float deltaT = shieldStartLevel / shieldRechargeRate;
            const float energyAvailable = shieldStartLevel / 4F;

            _ship.RequestEnergy(Arg.Any<float>()).Returns(energyAvailable);

            _shield.Damage(damage);

            _shield.Recharge(deltaT);

            Assert.AreEqual(shieldStartLevel - damage + energyAvailable, _shield.Level);
        }
    }
}
