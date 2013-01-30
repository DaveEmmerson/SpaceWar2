using System;
using System.Globalization;
using System.Linq;
using DEMW.SpaceWar2.Core.Controls;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.Core.Utils;
using DEMW.SpaceWar2.Core.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Core
{
    internal class GameEngine
    {
        //private const float Speed = 100f;

        //private readonly IContentManager _contentManager;
        //private readonly IGraphicsDevice _graphicsDevice;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGravitySimulator _gravitySimulator;
        private readonly IUniverse _universe;
        //private readonly IShipComponentFactory _shipComponentFactory;
        //private readonly IGraphicsFactory _graphicsFactory;
        
        private readonly IKeyboardHandler _keyboardHandler;
        //private readonly ControllerFactory _controllerFactory;

        //private readonly IDrawingManager _drawingManager;

        private readonly IActionHandler _actionHandler;
		
        private bool _paused;
        

        //private InfoBar _infoBar;
        //private Camera _camera;
        		
        //private Effect _effect;

        internal GameEngine(IUniverse universe, IGravitySimulator gravitySimulator, IGameObjectFactory gameObjectFactory, IKeyboardHandler keyboardHandler, IActionHandler actionHandler)
        {
        //    _contentManager = new ContentManagerWrapper(Content.ServiceProvider, "Content");
        //    var graphicsDeviceManager = new GraphicsDeviceManager(this);
        //    _graphicsDevice = new GraphicsDeviceWrapper(graphicsDeviceManager);

            _universe = universe;
        //    _drawingManager = new DrawingManager(_universe);
            _gravitySimulator = gravitySimulator;
        //    _shipComponentFactory = new ShipComponentFactory();
        //    _graphicsFactory = new GraphicsFactory();

            _gameObjectFactory = gameObjectFactory;
            
            _keyboardHandler = keyboardHandler;
            _actionHandler = actionHandler;
        //    _controllerFactory = new ControllerFactory(_keyboardHandler);
            actionHandler.RegisterTriggerAction(Keys.Space, () => _paused = !_paused);
        }

        public bool Paused
        {
            get { return _paused; }
        }

        //private void ResetGame()
        //{
        //    _gameObjectFactory.DestroyAll(x=>true);

        //    _drawingManager.ResetCamera(_universe);
        //    _camera = _drawingManager.ActiveCamera;
            
        //    if (_effect != null)
        //    {
        //        _effect.Parameters["Projection"].SetValue(_camera.Projection);
        //        _effect.Parameters["View"].SetValue(_camera.View);
        //    }

        //    var initialDistance = new Vector2(0, 100);
        //    var initialVelocity = new Vector2(Speed, 0);

        //    _gameObjectFactory.CreateShip("ship 1", initialDistance, initialVelocity, Color.Red, _controllerFactory.Controller1);
        //    _gameObjectFactory.CreateShip("ship 2", -initialDistance, -initialVelocity, Color.Blue, _controllerFactory.Controller2);

        //    _gameObjectFactory.CreateSun(Vector2.Zero, Color.Red, Speed * Speed);
        //    _gameObjectFactory.CreateSun(new Vector2(200, 0), Color.Orange, Speed * Speed);
        //    _gameObjectFactory.CreateSun(new Vector2(-200, 0), Color.OrangeRed, Speed * Speed);
        //}
        
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

        ///// <summary>
        ///// This is called when the game should draw itself.
        ///// </summary>
        ///// <param name="gameTime">Provides a snapshot of timing values.</param>
        //protected override void Draw(GameTime gameTime)
        //{
        //    GraphicsDevice.Clear(Color.Black);

        //    var gameObjects = _gameObjectFactory.GameObjects;

        //    _effect.Parameters["View"].SetValue(_camera.View);
        //    _effect.Parameters["Projection"].SetValue(_camera.Projection);

        //    gameObjects.ForEach<IGameObject, IGameObject>(gameObject =>
        //    {
        //        _effect.Parameters["World"].SetValue(Matrix.CreateTranslation(new Vector3(gameObject.Position, 0.0f)));

        //        foreach (var pass in _effect.CurrentTechnique.Passes)
        //        {
        //            pass.Apply();
        //            gameObject.Draw(_graphicsDevice);
        //        }
        //    });

        //    _drawingManager.DrawGameObjects();

        //    _infoBar.CursorPosition = new Vector2(10,10);
        //    gameObjects.ForEach<IGameObject, IShip>(x =>
        //                                                {
        //                                                    var debugDetails = x.DebugDetails;
        //                                                    _infoBar.DrawString(debugDetails + "\r\n\r\n\r\n\r\n");
        //                                                });

        //    var universeDimensions = String.Format(CultureInfo.InvariantCulture, "Universe - width: {0}, height: {1}", _universe.Volume.Width, _universe.Volume.Height);
        //    _infoBar.DrawString(universeDimensions);

        //    base.Draw(gameTime);
        //}
    }
}
