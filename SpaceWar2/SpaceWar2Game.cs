using DEMW.SpaceWar2.Core;
using DEMW.SpaceWar2.Core.Controls;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.Core.Utils;
using DEMW.SpaceWar2.Core.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Globalization;
using System.Linq;

namespace DEMW.SpaceWar2
{
    internal class SpaceWar2Game : Game
    {
        private readonly IContentManager _contentManager;
        private readonly IGraphicsDevice _graphicsDevice;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGravitySimulator _gravitySimulator;
        private readonly IUniverse _universe;
        private readonly IShipComponentFactory _shipComponentFactory;
        private readonly IGraphicsFactory _graphicsFactory;
        
        private readonly IKeyboardHandler _keyboardHandler;

        private readonly IDrawingManager _drawingManager;

        private readonly GameEngine _gameEngine;
        
        private InfoBar _infoBar;
        		
        private Effect _effect;

        internal SpaceWar2Game()
        {
            _contentManager = new ContentManagerWrapper(Content.ServiceProvider, "Content");
            var graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDevice = new GraphicsDeviceWrapper(graphicsDeviceManager);

            _universe = Universe.CreateDefault();
            _drawingManager = new DrawingManager(_universe);
            _gravitySimulator = new GravitySimulator();
            _shipComponentFactory = new ShipComponentFactory();
            _graphicsFactory = new GraphicsFactory();

            _gameObjectFactory = new GameObjectFactory(_contentManager, _graphicsFactory, _gravitySimulator, _drawingManager, _universe, _shipComponentFactory);
            
            _keyboardHandler = new KeyboardHandler(new KeyboardWrapper());

            var actionHandler = new ActionHandler(_keyboardHandler);

            _gameEngine = new GameEngine(_universe, _gravitySimulator, _gameObjectFactory, _keyboardHandler, actionHandler, _drawingManager);

            SetUpActions(actionHandler);
        }

        //TODO Could set these actions based on key bindings loaded from a file.
        //Could also be moved into GameEngine or Somewhere else (ActionHandler? BindingsConfigurator?)
        private void SetUpActions(IActionHandler actionHandler)
        {
            actionHandler.RegisterTriggerAction(Keys.Escape, _gameEngine.ResetGame);

            actionHandler.RegisterTriggerAction(Keys.X, () =>
            {
                var ship = _gameObjectFactory.GameObjects.OfType<Ship>().SingleOrDefault(x => x.Name == "ship 1");
                if (ship != null)
                {
                    ship.Damage(10f);
                }
            });

            actionHandler.RegisterContinuousAction(Keys.T, () => _drawingManager.MoveCamera(Vector3.Forward));
            actionHandler.RegisterContinuousAction(Keys.Y, () => _drawingManager.MoveCamera(Vector3.Up));
            actionHandler.RegisterContinuousAction(Keys.U, () => _drawingManager.ZoomCamera(-10));
            actionHandler.RegisterContinuousAction(Keys.J, () => _drawingManager.ZoomCamera(10));
            actionHandler.RegisterContinuousAction(Keys.I, () => _universe.Volume.Contract(10));
            actionHandler.RegisterContinuousAction(Keys.K, () => _universe.Volume.Expand(10));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _gameEngine.ResetGame();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _effect = _contentManager.Load<Effect>("Effects/HLSLTest");

            _effect.Parameters["AmbientColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            _effect.Parameters["AmbientProportion"].SetValue(0.5f);

            _effect.CurrentTechnique = _effect.Techniques["TestTechnique"];

            var spriteBatch = new SpriteBatchWrapper(_graphicsDevice);
            var spriteFont = _contentManager.Load<SpriteFont>("Fonts/Segoe UI Mono");
            var spriteFontWrapper = new SpriteFontWrapper(spriteFont);
            
            _infoBar = new InfoBar(spriteBatch, spriteFontWrapper);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _gameEngine.ExecuteGameLoop(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _effect.Parameters["View"].SetValue(_drawingManager.CameraView);
            _effect.Parameters["Projection"].SetValue(_drawingManager.CameraProjection);

            //TODO maybe move this into DrawingManager
            ApplyTranslationsAndEffectsToGameObjects();
            _drawingManager.DrawGameObjects();

            DrawDebugInfomation();

            base.Draw(gameTime);
        }

        private void ApplyTranslationsAndEffectsToGameObjects()
        {
            _gameObjectFactory.GameObjects.ForEach<IGameObject, IGameObject>(gameObject =>
            {
                _effect.Parameters["World"].SetValue(Matrix.CreateTranslation(new Vector3(gameObject.Position, 0.0f)));

                foreach (var pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gameObject.Draw(_graphicsDevice);
                }
            });
        }

        private void DrawDebugInfomation()
        {
            _infoBar.CursorPosition = new Vector2(10, 10);
            _gameObjectFactory.GameObjects.ForEach<IGameObject, IShip>(x =>
            {
                var debugDetails = x.DebugDetails;
                _infoBar.DrawString(debugDetails + "\r\n\r\n\r\n\r\n");
            });

            var universeDimensions = String.Format(CultureInfo.InvariantCulture, "Universe - width: {0}, height: {1}",
                                                   _universe.Volume.Width, _universe.Volume.Height);

            _infoBar.DrawString(universeDimensions);
        }

        // This method only exists so that a dummy test can call it, which forces this 
        // assembly to be included in the coverage report. If there's a better way to 
        // do this, then this method can be removed
        public static void DummyTestMethod() { }
    }
}
