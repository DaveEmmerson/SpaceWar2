using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar2
{
    public class SpaceWar2Game : Game
    {
        private const float _speed = 100f;

        private readonly GraphicsDeviceManager _graphics;

        private bool _paused;

        private Viewport _viewport;
        private float _minX;
        private float _minY;
        private float _maxX;
        private float _maxY;

        private SpriteBatch _spriteBatch;

        private InfoBar _infoBar;
        
        private readonly Vector2 _initialDistance;
        private readonly Vector2 _initialVelocity;

        private readonly KeyboardHandler _keyboardHandler;
        private readonly GameObjectFactory _gameObjectFactory;
		private readonly GravitySimulator _gravitySimulator;
				
        private Effect _effect;
        private Matrix _view;
        private Matrix _projection;
        
        public SpaceWar2Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _gravitySimulator = new GravitySimulator();
            _gameObjectFactory = new GameObjectFactory(_graphics, _gravitySimulator);
            
            Content.RootDirectory = "Content";
            
            _keyboardHandler = new KeyboardHandler();

            _initialDistance = new Vector2(0,100);
            _initialVelocity = new Vector2(_speed,0);
        }

        private void ResetGame()
        {
            _gameObjectFactory.DestroyAll(x=>true);
            _gravitySimulator.Clear();

            var sunPosition = new Vector2(_viewport.Width/2f,_viewport.Height/2f);
            _gameObjectFactory.CreateSun(sunPosition, Color.Red, _speed * _speed);

            var controller1 = CreateController1();
            var ship1Position = sunPosition + _initialDistance;
            _gameObjectFactory.CreateShip("ship 1", ship1Position, _initialVelocity, Color.Red, controller1);

            var controller2 = CreateController2();
            var ship2Position = sunPosition - _initialDistance;
            _gameObjectFactory.CreateShip("ship2", ship2Position, -_initialVelocity, Color.Blue, controller2);

            sunPosition = new Vector2(_viewport.Width / 2f + 200, _viewport.Height / 2f);
            _gameObjectFactory.CreateSun(sunPosition, Color.Orange, _speed * _speed);

            sunPosition = new Vector2(_viewport.Width / 2f - 200, _viewport.Height / 2f);
            _gameObjectFactory.CreateSun(sunPosition, Color.OrangeRed, _speed * _speed);
        }

        private IShipController CreateController1()
        {
            var controller = new KeyboardController(_keyboardHandler);

            controller.SetMapping(Keys.Up, ShipAction.Thrust);
            controller.SetMapping(Keys.Left, ShipAction.TurnLeft);
            controller.SetMapping(Keys.Right, ShipAction.TurnRight);
            controller.SetMapping(Keys.Down, ShipAction.ReverseThrust);
            controller.SetMapping(Keys.RightControl, ShipAction.FireProjectile);

            return controller;
        }

        private IShipController CreateController2()
        {
            var controller = new KeyboardController(_keyboardHandler);

            controller.SetMapping(Keys.W, ShipAction.Thrust);
            controller.SetMapping(Keys.A, ShipAction.TurnLeft);
            controller.SetMapping(Keys.D, ShipAction.TurnRight);
            controller.SetMapping(Keys.S, ShipAction.ReverseThrust);
            controller.SetMapping(Keys.R, ShipAction.FireProjectile);

            return controller;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _view = Matrix.CreateLookAt(new Vector3(0, 0, 1.0f), Vector3.Zero, Vector3.Up);

            _viewport = _graphics.GraphicsDevice.Viewport;

            _minX = _viewport.X;
            _minY = _viewport.Y;
            _maxX = _minX + _viewport.Width;
            _maxY = _minY + _viewport.Height;

            _projection = Matrix.CreateOrthographicOffCenter(
                _minX, _maxX,
                _maxY, _minY,
                -1000.0f, 1000.0f
            );

            ResetGame();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _effect = Content.Load<Effect>("HLSLTest");

            _effect.Parameters["View"].SetValue(_view);
            _effect.Parameters["Projection"].SetValue(_projection);

            _effect.Parameters["AmbientColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            _effect.Parameters["AmbientProportion"].SetValue(0.5f);

            _effect.CurrentTechnique = _effect.Techniques["TestTechnique"];

            _infoBar = new InfoBar(_spriteBatch);
            _infoBar.LoadContent(Content);
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

            if (_keyboardHandler.IsNewlyPressed(Keys.Space))
            {
                _paused = !_paused;
            }

            if (_keyboardHandler.IsNewlyPressed(Keys.Escape))
            {
                ResetGame();
            }

            if (_keyboardHandler.IsNewlyPressed(Keys.X))
            {
                var ship = _gameObjectFactory.GameObjects.OfType<Ship>().SingleOrDefault(x => x.Name == "ship 1");
                if (ship != null)
                {
                    ship.Damage(10);
                }
            }

            if (!_paused)
            {
                _gameObjectFactory.DestroyAll(obj => obj.Expired);
                var gameObjects = _gameObjectFactory.GameObjects;

                gameObjects.ForEach<IGameObject, Ship>(ship => ship.Acceleration = Vector2.Zero);
                
                _gravitySimulator.Simulate();
                
                gameObjects.ForEach<IGameObject, Ship>(ship =>
                {
                    ScreenConstraint(ship);
                    ship.Update(gameTime);
                });
            }

            base.Update(gameTime);
        }

        private void ScreenConstraint(Ship ship)
        {
            Vector2 position = ship.Position;

            if (position.X < _minX)
            {
                position.X = _maxX + position.X % _viewport.Width;
            }

            if (position.X > _maxX)
            {
                position.X = _minX - position.X % _viewport.Width;
            }

            if (position.Y < _minY)
            {
                position.Y = _maxY + position.Y % _viewport.Height;
            }

            if (position.Y > _maxY)
            {
                position.Y = _minY - position.Y % _viewport.Height;
            }

            ship.Position = position;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _infoBar.Reset();
            
            var gameObjects = _gameObjectFactory.GameObjects;

            gameObjects.ForEach<IGameObject, IGameObject>(gameObject =>
            {
                _effect.Parameters["World"].SetValue(Matrix.CreateTranslation(new Vector3(gameObject.Position, 0.0f)));

                _effect.CurrentTechnique.Passes[0].Apply();

                gameObject.Draw();
            });

            gameObjects.ForEach<IGameObject, Ship>(_infoBar.DrawShipInfo);            

            base.Draw(gameTime);
        }
    }
}
