using NUnit.Framework;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using NSubstitute;

namespace DEMW.SpaceWar2Tests.GameObjects.ShipComponents
{
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
        public void Small_Damage_Reduces_Shields_With_No_Damage_Remaining()
        {
            var damage = shieldStartLevel / 10F;

            var damageRemaining = _shield.Damage(damage);

            Assert.AreEqual(shieldStartLevel - damage, _shield.Level);
            Assert.AreEqual(0F, damageRemaining);
        }

        [Test]
        public void Large_Damage_Reduces_Shields_To_Zero_With_Damage_Remaining()
        {
            var excessDamage = 50F;
            var damage = shieldStartLevel + excessDamage;

            var damageRemaining = _shield.Damage(damage);

            Assert.AreEqual(0, _shield.Level);
            Assert.AreEqual(excessDamage, damageRemaining);

        }

        [Test]
        public void Damaged_Shield_Recharges_By_Small_Amount_Correctly()
        {
            var damage = shieldStartLevel / 2F;
            var deltaT = shieldStartLevel / shieldRechargeRate / 10F;

            _shield.Damage(damage);

            _shield.Recharge(deltaT);

            var expectedIncrease = deltaT * shieldRechargeRate;
            Assert.AreEqual(shieldStartLevel - damage + expectedIncrease, _shield.Level);
            _ship.Received().RequestEnergy(expectedIncrease);
        }

        [Test]
        public void Damaged_Shield_Recharges_By_Large_Amount_Correctly()
        {
            var damage = shieldStartLevel / 2F;
            var deltaT = shieldStartLevel / shieldRechargeRate * 2F;

            _shield.Damage(damage);

            _shield.Recharge(deltaT);

            Assert.AreEqual(shieldStartLevel, _shield.Level);
            _ship.Received().RequestEnergy(damage);
        }

    }
}
