using System;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects.ShipComponents
{
    [TestFixture]
    class EnergyStoreTests
    {
        private EnergyStore _energyStore;
        private const float maxLevel = 100F;
        private const float rechargeRate = 0.0001F;

        [SetUp]
        public void SetUp()
        {
            _energyStore = new EnergyStore(maxLevel, rechargeRate);
        }

        [Test]
        public void Level_is_maximum_initially()
        {
            Assert.AreEqual(maxLevel, _energyStore.Level);
        }

        [Test]
        public void Request_energy_with_sufficient_energy_returns_correct_amount_and_reduces_level()
        {
            const float energyRequired = maxLevel / 2;

            var energyReceived = _energyStore.RequestEnergy(energyRequired);

            Assert.AreEqual(energyRequired, energyReceived, "Energy received did not equal energy requested.");
            Assert.AreEqual(maxLevel - energyReceived, _energyStore.Level, "Energy level was not reduced by energy received.");
        }

        [Test]
        public void Request_energy_with_only_partial_energy_available_returns_all_available_and_level_reduces_to_zero()
        {
            const float energyRequired = maxLevel * 2;

            var energyReceived = _energyStore.RequestEnergy(energyRequired);

            Assert.AreEqual(energyReceived, maxLevel, "Energy received did not equal the available energy.");
            Assert.AreEqual(0F, _energyStore.Level, "Energy level remaining was not zero.");
        }

        [Test]
        public void Request_energy_with_no_energy_available_returns_zero_and_level_remains_zero()
        {
            const float energyRequired = 50F;

            _energyStore.RequestEnergy(maxLevel);

            var energyReceived = _energyStore.RequestEnergy(energyRequired);

            Assert.AreEqual(0F, energyReceived, "Energy received was not zero.");
            Assert.AreEqual(0F, _energyStore.Level, "Energy level did not remain zero.");
        }

        [Test]
        public void Request_energy_with_zero_argument_returns_zero_and_does_not_change_level()
        {
            var energyReceived = _energyStore.RequestEnergy(0F);

            Assert.AreEqual(0F, energyReceived, "Energy received was not zero.");
            Assert.AreEqual(maxLevel, _energyStore.Level, "Energy level changed.");
        }

        [Test]
        public void Request_energy_with_negative_argument_throws_exception()
        {
            var exception = Assert.Throws<ArgumentException>(() => _energyStore.RequestEnergy(-10F));

            Assert.AreEqual("Must not be negative.\r\nParameter name: amountRequested", exception.Message);
            Assert.AreEqual(maxLevel, _energyStore.Level);
        }

        [Test]
        public void Recharge_increases_level_by_recharge_rate_deltaT()
        {
            const float deltaT = 25F / rechargeRate;
            const float energyToDiscard = 50F;

            _energyStore.RequestEnergy(energyToDiscard);

            _energyStore.Recharge(deltaT);

            Assert.AreEqual(maxLevel - energyToDiscard + deltaT * rechargeRate, _energyStore.Level);
        }

        [Test]
        public void Recharge_only_increases_up_to_max_level()
        {
            const float deltaT = 75F / rechargeRate;
            const float energyToDiscard = 50F;

            _energyStore.RequestEnergy(energyToDiscard);

            _energyStore.Recharge(deltaT);

            Assert.AreEqual(maxLevel, _energyStore.Level);
        }

        [Test]
        public void Recharge_with_zero_parameter_does_nothing()
        {
            const float energyToDiscard = 50F;

            _energyStore.RequestEnergy(energyToDiscard);

            _energyStore.Recharge(0F);

            Assert.AreEqual(maxLevel - energyToDiscard, _energyStore.Level);
        }

        [Test]
        public void Recharge_with_negative_parameter_throws_exception()
        {
            var exception = Assert.Throws<ArgumentException>(() => _energyStore.Recharge(-10F));
            Assert.AreEqual("Must not be negative.\r\nParameter name: deltaT", exception.Message);
        }
    }
}
