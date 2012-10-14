using System.Linq;
using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2
{
    public class SpaceWar2Game : Game
    {
        private const float Speed = 100f;

        private readonly GraphicsDeviceManager _graphics;
        private readonly GameObjectFactory _gameObjectFactory;
        private readonly GravitySimulator _gravitySimulator;
        private readonly Universe _universe;

        private readonly KeyboardHandler _keyboardHandler;
        private readonly ControllerFactory _controllerFactory;

        private readonly DrawingManager _drawingManager;
		
        private bool _paused;

        private InfoBar _infoBar;
        private Camera _camera;
        		
        private Effect _effect;
        
        public SpaceWar2Game()
        {
            Content.RootDirectory = "Content";
            _graphics = new GraphicsDeviceManager(this);
            _universe = Universe.GetDefault();
            _drawingManager = new DrawingManager(_universe);
            _gravitySimulator = new GravitySimulator();
            _gameObjectFactory = new GameObjectFactory(Content, _graphics, _gravitySimulator, _drawingManager, _universe);
            
            _keyboardHandler = new KeyboardHandler();
            _controllerFactory = new ControllerFactory(_keyboardHandler);
        }

        private void ResetGame()
        {
            _gameObjectFactory.DestroyAll(x=>true);

            _camera = Camera.GetDefault(_universe);
            _drawingManager.ActiveCamera = _camera;
            
            if (_effect != null)
            {
                _effect.Parameters["Projection"].SetValue(_camera.Projection);
                _effect.Parameters["View"].SetValue(_camera.View);
            }

            var initialDistance = new Vector2(0, 100);
            var initialVelocity = new Vector2(Speed, 0);

            _gameObjectFactory.CreateShip("ship 1", initialDistance, initialVelocity, Color.Red, _controllerFactory.Controller1);
            _gameObjectFactory.CreateShip("ship 2", -initialDistance, -initialVelocity, Color.Blue, _controllerFactory.Controller2);
            
            _gameObjectFactory.CreateSun(Vector2.Zero, Color.Red, Speed * Speed);
            _gameObjectFactory.CreateSun(new Vector2(200, 0), Color.Orange, Speed * Speed);
            _gameObjectFactory.CreateSun(new Vector2(-200, 0), Color.OrangeRed, Speed * Speed);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ResetGame();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _effect = Content.Load<Effect>("Effects/HLSLTest");

            _effect.Parameters["View"].SetValue(_camera.View);
            _effect.Parameters["Projection"].SetValue(_camera.Projection);

            _effect.Parameters["AmbientColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            _effect.Parameters["AmbientProportion"].SetValue(0.5f);

            _effect.CurrentTechnique = _effect.Techniques["TestTechnique"];

            var spriteBatch = new SpriteBatch(GraphicsDevice);
            _infoBar = new InfoBar(spriteBatch, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _keyboardHandler.UpdateKeyboardState();
            CheckNonGameKeys();
            CheckDebugKeys();

            if (!_paused)
            {
                _gameObjectFactory.DestroyAll(obj => obj.Expired);
                _gravitySimulator.Simulate();
                _gameObjectFactory.GameObjects.ForEach<IGameObject, GameObject>(x => x.Update(gameTime));
                _universe.Update();
            }

            base.Update(gameTime);
        }

        private void CheckNonGameKeys()
        {
            if (_keyboardHandler.IsNewlyPressed(Keys.Space))
            {
                _paused = !_paused;
            }

            if (_keyboardHandler.IsNewlyPressed(Keys.Escape))
            {
                ResetGame();
            }
        }

        private void CheckDebugKeys()
        {
            if (_keyboardHandler.IsNewlyPressed(Keys.X))
            {
                var ship = _gameObjectFactory.GameObjects.OfType<Ship>().SingleOrDefault(x => x.Name == "ship 1");
                if (ship != null)
                {
                    ship.Damage(10);
                }
            }

            if (_keyboardHandler.IsPressed(Keys.T))
            {
                _camera.Pan(Vector3.Forward);
            }
            
            if (_keyboardHandler.IsPressed(Keys.Y))
            {
                _camera.Pan(Vector3.Up);
            }

            if (_keyboardHandler.IsPressed(Keys.U))
            {
                _camera.Zoom(-10);
            }

            if (_keyboardHandler.IsPressed(Keys.J))
            {
                _camera.Zoom(10);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var gameObjects = _gameObjectFactory.GameObjects;

            _effect.Parameters["View"].SetValue(_camera.View);
            _effect.Parameters["Projection"].SetValue(_camera.Projection);

            gameObjects.ForEach<IGameObject, IGameObject>(gameObject =>
            {
                _effect.Parameters["World"].SetValue(Matrix.CreateTranslation(new Vector3(gameObject.Position, 0.0f)));

                foreach (var pass in _effect.CurrentTechnique.Passes)
                {

                    pass.Apply();

                    gameObject.Draw();

                }
            });

            _drawingManager.DrawGameObjects();

            _infoBar.Reset();
            gameObjects.ForEach<IGameObject, Ship>(_infoBar.DrawShipInfo);            

            base.Draw(gameTime);
        }
    }
}
