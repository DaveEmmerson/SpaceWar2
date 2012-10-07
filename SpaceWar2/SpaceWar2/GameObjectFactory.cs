using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SpaceWar2.Physics;

namespace SpaceWar2
{
    internal class GameObjectFactory
    {
        private readonly IList<IGameObject> _gameObjects;
        private readonly GraphicsDeviceManager _graphics;
        private readonly GravitySimulator _gravitySimulator;

        public GameObjectFactory(GraphicsDeviceManager graphics, GravitySimulator gravitySimulator)
        {
            _graphics = graphics;
            _gravitySimulator = gravitySimulator;
            _gameObjects = new List<IGameObject>();
        }

        public IList<IGameObject> GameObjects { get { return _gameObjects; } }

        internal Sun CreateSun(Vector2 position, Color color, float mass)
        {
            var sun = new Sun(_graphics, position, 25, color, 32, mass);
            
            _gravitySimulator.RegisterSource(sun);
            _gameObjects.Add(sun);
          
            return sun;
        }

        internal Ship CreateShip(string name, Vector2 position, Vector2 velocity, Color color, IShipController controller)
        {
            var ship = new Ship(name, _graphics, position, 16, color, 16)
            {
                Velocity = velocity,
                Controller = controller,
                ShowArrows = true
            };

            _gravitySimulator.RegisterParticipant(ship);
            _gameObjects.Add(ship);

            return ship;
        }

        internal void DestroyAll(Predicate<IGameObject> match)
        {
            var itemsToDestory = GameObjects.Where(x => match(x)).ToList();
            
            foreach (var item in itemsToDestory)
            {
                _gravitySimulator.UnRegister(item);
                GameObjects.Remove(item);
            }
        }
    }
}
