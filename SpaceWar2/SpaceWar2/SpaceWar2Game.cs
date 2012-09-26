using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceWar2
{
    public class SpaceWar2Game : Game
    {
        const float Speed = 10f;

        private readonly GraphicsDeviceManager _graphics;

        private bool _paused;

        private Viewport _viewport;
        private float _minX;
        private float _minY;
        private float _maxX;
        private float _maxY;

        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;

        private readonly List<IGameObject> _gameObjects;

        private InfoBar _infoBar;

        private Sun _sun;
        private Ship _ship1;
        private Ship _ship2;

        private readonly Vector2 _initialDistance;
        private readonly Vector2 _initialVelocity;

        private readonly KeyboardHandler _keyboardHandler;
        private readonly GameObjectFactory _gameObjectFactory;
        
        public SpaceWar2Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _gameObjectFactory = new GameObjectFactory(_graphics);

            Content.RootDirectory = "Content";

            _gameObjects = new List<IGameObject>();

            _keyboardHandler = new KeyboardHandler();

            _initialDistance = new Vector2(0,100);
            _initialVelocity = new Vector2(10f * Speed,0);
        }

        private void ResetGame()
        {

            _gameObjects.Clear();

            var viewport = _graphics.GraphicsDevice.Viewport;
            var sunPosition = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            _sun = _gameObjectFactory.CreateSun(sunPosition, Color.White, Speed * Speed);

            var controller1 = CreateController1();
            var ship1Position = _sun.Position + _initialDistance;
            _ship1 = _gameObjectFactory.CreateShip("ship 1", ship1Position, _initialVelocity, Color.Red, controller1);

            var controller2 = CreateController2();
            var ship2Position = _sun.Position - _initialDistance;
            _ship2 = _gameObjectFactory.CreateShip("ship2", ship2Position, -_initialVelocity, Color.Blue, controller2);
            
            _gameObjects.Add(_sun);
            _gameObjects.Add(_ship1);
            _gameObjects.Add(_ship2);
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
            _basicEffect = new BasicEffect(_graphics.GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = Matrix.CreateOrthographicOffCenter(
                    0, _graphics.GraphicsDevice.Viewport.Width, // left, right
                    _graphics.GraphicsDevice.Viewport.Height, 0, // bottom, top
                    0, 1)
            };
            
            ResetGame();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _infoBar = new InfoBar(_spriteBatch);
            _infoBar.LoadContent(Content);

            _viewport = _graphics.GraphicsDevice.Viewport;

            _minX = _viewport.X;
            _minY = _viewport.Y;
            _maxX = _viewport.Width;
            _maxY = _viewport.Height;
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
                _ship1.Damage(10);
            }

            if (!_paused)
            {
                _gameObjects.RemoveAll(obj => obj.Expired);

                _gameObjects.ForEach<IGameObject, Ship>(ship =>
                {
                    ship.Acceleration = Vector2.Zero;
                    ApplyGravity(ship, _sun);
                    ship.Update(gameTime);
                    ScreenConstraint(ship);
                });
           }

            base.Update(gameTime);
        }

        private static void ApplyGravity(Ship smallObject, IMassive massiveObject)
        {
            Vector2 diff = massiveObject.Position - smallObject.Position;

            if (diff.Length() > smallObject.Radius + massiveObject.Radius)
            {
                diff.Normalize();
                smallObject.Acceleration += diff * massiveObject.Mass / diff.LengthSquared();
            }
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

            _basicEffect.CurrentTechnique.Passes[0].Apply();

            _infoBar.Reset();
            
            _gameObjects.ForEach(gameObject => gameObject.Draw());
            _gameObjects.ForEach<IGameObject, Ship>(_infoBar.DrawShipInfo);            

            base.Draw(gameTime);
        }
    }
}
