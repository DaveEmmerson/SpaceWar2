using System;

namespace DEMW.SpaceWar2.Core.GameObjects.ShipComponents
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
                throw new ArgumentException("Must not be negative.", "amountRequested");
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
                throw new ArgumentException("Must not be negative.", "deltaT");
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
