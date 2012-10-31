﻿using System;

namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    public class Hull
    {
        private readonly IGameObject _ship;
        private readonly float _maxLevel;

        public float Level { get; private set; }

        public Hull(IGameObject ship, float maxLevel)
        {
            _ship = ship;
            _maxLevel = maxLevel;

            Level = maxLevel;
        }

        public void Damage(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Damage amount must not be negative.");
            }

            if (_ship.Expired)
            {
                return;
            }

            Level -= amount;

            if (Level < 0)
            {
                Level = 0;
                _ship.Expired = true;

            }

        }
    }
}