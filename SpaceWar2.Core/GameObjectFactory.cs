using DEMW.SpaceWar2.Core.Controls;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.Core.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DEMW.SpaceWar2.Core
{
    internal class GameObjectFactory : IGameObjectFactory
    {
        private readonly IList<IGameObject> _gameObjects;
        private readonly IContentManager _contentManager;
        private readonly IGraphicsFactory _graphicsFactory;
        private readonly IGravitySimulator _gravitySimulator;
        private readonly IUniverse _universe;
        private readonly IDrawingManager _drawingManager;
        private readonly IShipComponentFactory _shipComponentFactory;

        internal GameObjectFactory(IContentManager contentManager, IGraphicsFactory graphicsFactory, IGravitySimulator gravitySimulator, IDrawingManager drawingManager, IUniverse universe, IShipComponentFactory shipComponentFactory)
        {
            _contentManager = contentManager;
            _graphicsFactory = graphicsFactory;
            _gravitySimulator = gravitySimulator;
            _drawingManager = drawingManager;
            _universe = universe;

            _shipComponentFactory = shipComponentFactory;
            _gameObjects = new List<IGameObject>();
        }

        public IList<IGameObject> GameObjects { get { return _gameObjects; } }

        public IGameObject CreateSun(Vector2 position, Color color, float mass)
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

        public IGameObject CreateShip(string name, Vector2 position, Vector2 velocity, Color color, IShipController controller)
        {
            var ship = new Ship(name, position, 16, color, _graphicsFactory, _shipComponentFactory)
            {
                Velocity = velocity,
                Model = _contentManager.Load<Model>("Models/Saucer")
            };
            ship.SetController(controller);
            ship.SetShowArrows(true);

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
                _gravitySimulator.Unregister(item);
                _universe.Unregister(item);
                _drawingManager.Unregister(item);
                GameObjects.Remove(item);
            }
        }
    }
}
