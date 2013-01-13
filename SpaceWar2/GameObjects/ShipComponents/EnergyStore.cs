using System;

namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{    
    internal class EnergyStore : IEnergyStore
    {
        public float Level { get; private set; }

        private readonly float _maxLevel;
        private readonly float _rechargeRate;

        internal EnergyStore(float maxLevel, float rechargeRate)
        {
            _maxLevel = maxLevel;
            _rechargeRate = rechargeRate;
            Level = maxLevel;
        }

        public float RequestEnergy(float amountRequested)
        {
            if (amountRequested < 0F)
            {
                throw new ArgumentException("Amount requested must not be negative.");
            }

            if (!(amountRequested > 0F))
            {
                return 0F;
            }

            var amountAchieved = amountRequested > Level ? Level : amountRequested;

            Level -= amountAchieved;

            return amountAchieved;
        }

        public void Recharge(float deltaT)
        {
            if (deltaT < 0)
            {
                throw new ArgumentException("deltaT must not be negative.");
            }

            if (!(deltaT > 0))
            {
                return;
            }

            Level += deltaT * _rechargeRate;

            if (Level > _maxLevel)
            {
                Level = _maxLevel;

            }
        }
    }
}
