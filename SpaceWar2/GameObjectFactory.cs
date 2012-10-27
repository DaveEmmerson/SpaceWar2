using System;
using System.Collections.Generic;
using System.Linq;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using DEMW.SpaceWar2.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2
{
    internal class GameObjectFactory
    {
        private readonly IList<IGameObject> _gameObjects;
        private readonly ContentManager _contentManager;
        private readonly GraphicsDeviceManager _graphics;
        private readonly GravitySimulator _gravitySimulator;
        private readonly Universe _universe;
        private readonly DrawingManager _drawingManager;

        public GameObjectFactory(ContentManager contentManager,GraphicsDeviceManager graphics, GravitySimulator gravitySimulator, DrawingManager drawingManager, Universe universe)
        {
            _contentManager = contentManager;
            _graphics = graphics;
            _gravitySimulator = gravitySimulator;
            _drawingManager = drawingManager;
            _universe = universe;
            _gameObjects = new List<IGameObject>();
        }

        public IList<IGameObject> GameObjects { get { return _gameObjects; } }

        internal Sun CreateSun(Vector2 position, Color color, float mass)
        {
            var sun = new Sun(position, 25, color, mass)
            {
                Model = _contentManager.Load<Model>("Models/Sun")
            };

            _gravitySimulator.RegisterSource(sun);
            _drawingManager.Register(sun);
            _gameObjects.Add(sun);
          
            return sun;
        }

        internal Ship CreateShip(string name, Vector2 position, Vector2 velocity, Color color, IShipController controller)
        {
            var ship = new Ship(name, _graphics, position, 16, color)
            {
                Velocity = velocity,
                Controller = controller,
                ShowArrows = true,
                Model = _contentManager.Load<Model>("Models/Saucer")
            };

            _gravitySimulator.RegisterParticipant(ship);
            _universe.Register(ship);
            _drawingManager.Register(ship);
            _gameObjects.Add(ship);

            return ship;
        }

        internal void DestroyAll(Predicate<IGameObject> match)
        {
            var itemsToDestory = GameObjects.Where(x => match(x)).ToList();
            
            foreach (var item in itemsToDestory)
            {
                _gravitySimulator.UnRegister(item);
                _universe.UnRegister(item);
                _drawingManager.UnRegister(item);
                GameObjects.Remove(item);
            }
        }
    }
}