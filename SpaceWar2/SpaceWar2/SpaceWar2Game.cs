using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceWar2.Physics;

namespace SpaceWar2
{
    public class SpaceWar2Game : Game
    {
        private const float Speed = 100f;

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
        private Texture2D _texture;
        private Model _model;

        private Camera _camera;
        
        public SpaceWar2Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _gravitySimulator = new GravitySimulator();
            _gameObjectFactory = new GameObjectFactory(_graphics, _gravitySimulator);
            
            Content.RootDirectory = "Content";
            
            _keyboardHandler = new KeyboardHandler();

            _initialDistance = new Vector2(0,100);
            _initialVelocity = new Vector2(Speed,0);
        }

        private void ResetGame()
        {
            _gameObjectFactory.DestroyAll(x=>true);

            _camera.Position = new Vector3(0, 0, 1f);
            _camera.Target = new Vector3(0, 0, 0);

            var sunPosition = new Vector2(_viewport.Width/2f,_viewport.Height/2f);
            _gameObjectFactory.CreateSun(sunPosition, Color.Red, Speed * Speed);

            if (_effect != null)
            {
                _effect.Parameters["Projection"].SetValue(_camera.Projection);
                _effect.Parameters["View"].SetValue(_camera.View);
            }

            var controller1 = CreateController1();
            var ship1Position = sunPosition + _initialDistance;
            _gameObjectFactory.CreateShip("ship 1", ship1Position, _initialVelocity, Color.Red, controller1);

            var controller2 = CreateController2();
            var ship2Position = sunPosition - _initialDistance;
            _gameObjectFactory.CreateShip("ship2", ship2Position, -_initialVelocity, Color.Blue, controller2);

            sunPosition = new Vector2(_viewport.Width / 2f + 200, _viewport.Height / 2f);
            _gameObjectFactory.CreateSun(sunPosition, Color.Orange, Speed * Speed);

            sunPosition = new Vector2(_viewport.Width / 2f - 200, _viewport.Height / 2f);
            _gameObjectFactory.CreateSun(sunPosition, Color.OrangeRed, Speed * Speed);
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

            _viewport = _graphics.GraphicsDevice.Viewport;

            _minX = _viewport.X;
            _minY = _viewport.Y;
            _maxX = _minX + _viewport.Width;
            _maxY = _minY + _viewport.Height;

            _camera = new Camera(new Vector3(0, 0, 1f), Vector3.Zero, _viewport);

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

            if (_keyboardHandler.IsPressed(Keys.T))
            {
                _camera.Pan(Vector3.Forward);
            }

            if (_keyboardHandler.IsPressed(Keys.Y))
            {

                _camera.Pan(Vector3.Up);

            }

            if (!_paused)
            {
                _gameObjectFactory.DestroyAll(obj => obj.Expired);

                _gravitySimulator.Simulate();

                var gameObjects = _gameObjectFactory.GameObjects;
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

            //var transforms = new Matrix[_model.Bones.Count];
            //_model.CopyAbsoluteBoneTransformsTo(transforms);

            //foreach (ModelMesh modelMesh in _model.Meshes)
            //{
            //    foreach (BasicEffect effect in modelMesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();
            //        effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            //        effect.World = transforms[modelMesh.ParentBone.Index];
            //        effect.TextureEnabled = true;
            //        effect.Texture = _texture;

            //        effect.View = _camera.View;
            //        effect.Projection = _camera.Projection;
            //    }

            //    modelMesh.Draw();
            //}

            _infoBar.Reset();
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
       
            gameObjects.ForEach<IGameObject, Ship>(_infoBar.DrawShipInfo);            

            base.Draw(gameTime);
        }
    }
}
