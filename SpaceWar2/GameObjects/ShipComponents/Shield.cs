using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    public class Shield
    {
        private IShip _ship;
        private float _maxLevel;
        private float _rechargeRate;
        private float _level;

        public float Level { get { return _level; } }

        public Shield(IShip ship, float maxLevel, float rechargeRate)
        {
            _ship = ship;
            _maxLevel = maxLevel;
            _rechargeRate = rechargeRate;
            _level = maxLevel;
        }

        public void Recharge(float deltaT) {

            var potentialIncrease = _rechargeRate * deltaT;

            var increaseRequired = Math.Min(_maxLevel - _level, potentialIncrease);

            _level = _level + _ship.RequestEnergy(increaseRequired);
        }

        public float Damage(float amount)
        {
            _level -= amount;

            if (_level < 0)
            {

                var damageRemaining = -_level;
                _level = 0;
                return damageRemaining;

            }

            return 0;
        }
                
    }
}
