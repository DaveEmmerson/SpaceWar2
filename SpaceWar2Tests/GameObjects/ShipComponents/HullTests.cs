using System;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects.ShipComponents
{
    [TestFixture]
    class HullTests
    {
        private const float maxLevel = 100F;

        private IGameObject _ship;
        private Hull _hull;

        [SetUp]
        public void SetUp()
        {
            _ship = Substitute.For<IGameObject>();
            _hull = new Hull(_ship, maxLevel);

        }

        [Test]
        public void Initial_Level_is_maxLevel()
        {
            Assert.AreEqual(maxLevel, _hull.Level);

        }

        [Test]
        public void Damage_has_no_effect_if_associated_game_object_is_Expired()
        {
            _ship.Expired.Returns(true);

            _hull.Damage(10F);

            Assert.AreEqual(maxLevel, _hull.Level);

        }

        [Test]
        public void Damage_reduces_Level()
        {
            const float damageReceived = maxLevel / 2;

            _hull.Damage(damageReceived);

            Assert.AreEqual(maxLevel / 2, _hull.Level);
        }

        [Test]
        public void Damage_does_nothing_if_passed_zero()
        {
            _hull.Damage(0F);

            Assert.AreEqual(maxLevel, _hull.Level);
        }

        [Test]
        public void Damage_only_reduces_Level_as_far_as_zero()
        {
            const float damageRequired = maxLevel * 2F;

            _hull.Damage(damageRequired);

            Assert.AreEqual(0F, _hull.Level);
        }

        [Test]
        public void Damage_throws_exception_if_passed_negative_parameter()
        {
            var exception = Assert.Throws<ArgumentException>(() => _hull.Damage(-10F));
            Assert.AreEqual("Must not be negative.\r\nParameter name: amount", exception.Message);
        }
    }
}
