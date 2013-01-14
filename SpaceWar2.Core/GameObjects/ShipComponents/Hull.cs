using System;

namespace DEMW.SpaceWar2.Core.GameObjects.ShipComponents
{
    internal class Hull : IHull
    {
        private readonly IGameObject _ship;

        public float Level { get; private set; }

        internal Hull(IGameObject ship, float maxLevel)
        {
            _ship = ship;

            Level = maxLevel;
        }

        public void Damage(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Must not be negative.", "amount");
            }

            if (_ship.Expired)
            {
                return;
            }

            Level -= amount;

            if (Level < 0)
            {
                Level = 0;
            }
        }
    }
}
