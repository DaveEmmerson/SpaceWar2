using DEMW.SpaceWar2.Core.Controls;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Core
{
    internal class GameEngine
    {
        private const float Speed = 100f;

        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGravitySimulator _gravitySimulator;
        private readonly IUniverse _universe;
        
        private readonly IKeyboardHandler _keyboardHandler;
        private readonly ControllerFactory _controllerFactory;

        private readonly IDrawingManager _drawingManager;

        private readonly IActionHandler _actionHandler;
		
        private bool _paused;
        
        internal GameEngine(IUniverse universe, IGravitySimulator gravitySimulator, IGameObjectFactory gameObjectFactory, IKeyboardHandler keyboardHandler, IActionHandler actionHandler, IDrawingManager drawingManager)
        {
            _universe = universe;
            _drawingManager = drawingManager;
            _gravitySimulator = gravitySimulator;

            _gameObjectFactory = gameObjectFactory;
            
            _keyboardHandler = keyboardHandler;
            _actionHandler = actionHandler;
            _controllerFactory = new ControllerFactory(_keyboardHandler);
            actionHandler.RegisterTriggerAction(Keys.Space, () => _paused = !_paused);
        }

        internal void ResetGame()
        {
            _gameObjectFactory.DestroyAll(x => true);

            _drawingManager.ResetCamera(_universe);

            var initialDistance = new Vector2(0, Speed);
            var initialVelocity = new Vector2(Speed, 0);

            _gameObjectFactory.CreateShip("ship 1", initialDistance, initialVelocity, Color.Red, _controllerFactory.Controller1);
            _gameObjectFactory.CreateShip("ship 2", -initialDistance, -initialVelocity, Color.Blue, _controllerFactory.Controller2);

            _gameObjectFactory.CreateSun(Vector2.Zero, Color.Red, Speed * Speed);
            _gameObjectFactory.CreateSun(new Vector2(200, 0), Color.Orange, Speed * Speed);
            _gameObjectFactory.CreateSun(new Vector2(-200, 0), Color.OrangeRed, Speed * Speed);
        }
        
        /// <summary>
        /// Simulates a frame of the game of length gameTime
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        internal void ExecuteGameLoop(GameTime gameTime)
        {
            _keyboardHandler.UpdateKeyboardState();
            _actionHandler.ProcessActions();

            if (!_paused)
            {
                _gameObjectFactory.DestroyAll(obj => obj.Expired);
                _gravitySimulator.Simulate();
                _gameObjectFactory.GameObjects.ForEach<IGameObject, GameObject>(x => x.Update(gameTime));
                _universe.Update();
            }
        }
    }
}
