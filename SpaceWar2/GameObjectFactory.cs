using System;
using System.Collections.Generic;
using System.Linq;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2
{
    public class GameObjectFactory
    {
        private readonly IList<IGameObject> _gameObjects;
        private readonly IContentManager _contentManager;
        private readonly IGraphicsDeviceManager _graphics;
        private readonly GravitySimulator _gravitySimulator;
        private readonly IUniverse _universe;
        private readonly IDrawingManager _drawingManager;
        private readonly IShipComponentFactory _shipComponentFactory;

        public GameObjectFactory(IContentManager contentManager, IGraphicsDeviceManager graphics, GravitySimulator gravitySimulator, IDrawingManager drawingManager, IUniverse universe)
        {
            _contentManager = contentManager;
            _graphics = graphics;
            _gravitySimulator = gravitySimulator;
            _drawingManager = drawingManager;
            _universe = universe;

            _shipComponentFactory = new ShipComponentFactory();
            _gameObjects = new List<IGameObject>();
        }

        public IList<IGameObject> GameObjects { get { return _gameObjects; } }

        public Sun CreateSun(Vector2 position, Color color, float mass)
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

        public Ship CreateShip(string name, Vector2 position, Vector2 velocity, Color color, IShipController controller)
        {
            var ship = new Ship(name, position, 16, color, _graphics, _shipComponentFactory)
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

        public void DestroyAll(Predicate<IGameObject> match)
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
