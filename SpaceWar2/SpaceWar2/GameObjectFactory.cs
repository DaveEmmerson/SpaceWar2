using System;
using System.Collections.Generic;
using System.Linq;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using DEMW.SpaceWar2.Graphics;

namespace DEMW.SpaceWar2
{
    internal class GameObjectFactory
    {
        private readonly IList<IGameObject> _gameObjects;
        private readonly GraphicsDeviceManager _graphics;
        private readonly GravitySimulator _gravitySimulator;
        private readonly ScreenManager _screenManager;

        public GameObjectFactory(GraphicsDeviceManager graphics, GravitySimulator gravitySimulator, ScreenManager screenManager)
        {
            _graphics = graphics;
            _gravitySimulator = gravitySimulator;
            _screenManager = screenManager;
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
            _screenManager.Register(ship);
            _gameObjects.Add(ship);

            return ship;
        }

        internal void DestroyAll(Predicate<IGameObject> match)
        {
            var itemsToDestory = GameObjects.Where(x => match(x)).ToList();
            
            foreach (var item in itemsToDestory)
            {
                _gravitySimulator.UnRegister(item);
                _screenManager.UnRegister(item);
                GameObjects.Remove(item);
            }
        }
    }
}
