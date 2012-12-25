using System;

namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    public class Shield : IShield
    {
        private readonly IShip _ship;
        private readonly float _maxLevel;
        private readonly float _rechargeRate;

        public float Level { get; private set; }

        public Shield(IShip ship, float maxLevel, float rechargeRate)
        {
            _ship = ship;
            _maxLevel = maxLevel;
            _rechargeRate = rechargeRate;
            
            Level = maxLevel;
        }
        
        public void Recharge(float deltaT) 
        {
            //Using this slightly weird logic to avoid (dodgy) == between two floats)
            if (!(Level < _maxLevel))
            {
                return;
            }

            var potentialIncrease = _rechargeRate * deltaT;

            var increaseRequired = Math.Min(_maxLevel - Level, potentialIncrease);

            Level = Level + _ship.RequestEnergy(increaseRequired);
        }

        public float Damage(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Damage amount must not be negative.");
            }

            Level -= amount;

            if (Level < 0)
            {

                var damageRemaining = -Level;
                Level = 0;
                return damageRemaining;

            }

            return 0;
        }
    }
}
