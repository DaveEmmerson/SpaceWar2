using DEMW.SpaceWar2.GameObjects;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Physics
{
    internal class Thruster
    {
        private readonly Vector2 _direction;
        private readonly Vector2 _position;
        private readonly Ship _ship;
        private readonly float _thrustEnergyCost;

        internal Thruster(Ship ship, Vector2 position, Vector2 direction, float thrustEnergyCost)
        {
            _ship = ship;
            _position = position;
            _direction = direction;
            _thrustEnergyCost = thrustEnergyCost;
        }

        internal void Engage()
        {
            float availableEnergy = _ship.RequestEnergy(_thrustEnergyCost);
            
            if (availableEnergy <= 0)
            {
                return;
            }
            
            var rotation = Matrix.CreateRotationZ(_ship.Rotation);
            
            Vector2 thrust = availableEnergy/_thrustEnergyCost * _direction;

            var force = Vector2.Transform(thrust, rotation);
            
            _ship.ApplyForce(new Force(force, _position));
        }
    }
}
