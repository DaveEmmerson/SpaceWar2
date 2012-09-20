using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpaceWar2
{
    /// <summary>
    /// This is the main type for your game
    /// Test comment
    /// </summary>
    public class SpaceWar2Game : Microsoft.Xna.Framework.Game
    {
        const float speed = 10f;

        private GraphicsDeviceManager graphics;

        private bool paused;

        private Viewport viewport;
        private float minX;
        private float minY;
        private float maxX;
        private float maxY;

        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;

        private List<IGameObject> gameObjects;

        private InfoBar infoBar;

        private Sun sun;
        private Ship ship1;
        private Ship ship2;

        private Vector2 initialDistanceVector;
        private Vector2 initialVelocityVector;

        private KeyboardHandler keyboardHandler;

        public SpaceWar2Game()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            gameObjects = new List<IGameObject>();

            keyboardHandler = new KeyboardHandler();

            initialDistanceVector = new Vector2(0,100);
            initialVelocityVector = new Vector2(10f * speed,0);
        }

        private void ResetGame()
        {

            sun = new Sun(

                graphics    : graphics,
                position    : new Vector2(200, 200),
                radius      : 32,
                lineColor   : Color.White,
                lineCount   : 32,
                mass        : speed * speed

            );

            sun.CentreToViewport();

            CreateShipOne();

            CreateShipTwo();

            gameObjects.Clear();
            gameObjects.Add(sun);
            gameObjects.Add(ship1);
            gameObjects.Add(ship2);

        }

        private void CreateShipTwo()
        {

            var controller = new KeyboardController(keyboardHandler);

            controller.SetMapping(Keys.W, ShipAction.Thrust);
            controller.SetMapping(Keys.A, ShipAction.TurnLeft);
            controller.SetMapping(Keys.D, ShipAction.TurnRight);
            controller.SetMapping(Keys.S, ShipAction.ReverseThrust);
            controller.SetMapping(Keys.R, ShipAction.FireProjectile);

            ship2 = new Ship(

                name: "ship 2",
                graphics: graphics,
                position: sun.Position - initialDistanceVector,
                radius: 16,
                lineColor: Color.Blue,
                lineCount: 16

            )
            {

                Velocity = -initialVelocityVector,
                Controller = controller,
                DrawArrows = true
            };


        }

        private void CreateShipOne()
        {
            var controller = new KeyboardController(keyboardHandler);

            controller.SetMapping(Keys.Up, ShipAction.Thrust);
            controller.SetMapping(Keys.Left, ShipAction.TurnLeft);
            controller.SetMapping(Keys.Right, ShipAction.TurnRight);
            controller.SetMapping(Keys.Down, ShipAction.ReverseThrust);
            controller.SetMapping(Keys.RightControl, ShipAction.FireProjectile);

            ship1 = new Ship(

                name: "ship 1",
                graphics: graphics,
                position: sun.Position + initialDistanceVector,
                radius: 16,
                lineColor: Color.Red,
                lineCount: 16

            )
            {
                Velocity = initialVelocityVector,
                Controller = controller,
                DrawArrows = true
            };
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(
                0, graphics.GraphicsDevice.Viewport.Width,     // left, right
                graphics.GraphicsDevice.Viewport.Height, 0,    // bottom, top
                0,1);                                         // near, far plane

            

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            infoBar = new InfoBar(spriteBatch);
            infoBar.LoadContent(Content);

            viewport = graphics.GraphicsDevice.Viewport;

            minX = viewport.X;
            minY = viewport.Y;
            maxX = viewport.Width;
            maxY = viewport.Height;

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

            keyboardHandler.UpdateKeyboardState();

            if (keyboardHandler.IsNewlyPressed(Keys.Space))
            {

                paused = !paused;

            }

            if (keyboardHandler.IsNewlyPressed(Keys.Escape))
            {

                ResetGame();

            }

            if (keyboardHandler.IsNewlyPressed(Keys.X))
            {

                ship1.Damage(10);

            }

            if (!paused)
            {

                gameObjects.RemoveAll(obj => obj.Expired);

                gameObjects.ForEach<IGameObject, Ship>(ship =>
                {

                    ship.Acceleration = Vector2.Zero;

                    applyGravity(ship, sun);

                    ship.Update(gameTime);

                    screenConstraint(ship);

                });


            }

            base.Update(gameTime);
        }

        private void applyGravity(Ship smallObject, IMassive massiveObject)
        {
            Vector2 diff = massiveObject.Position - smallObject.Position;

            if (diff.Length() > smallObject.Radius + massiveObject.Radius)
            {
                float d = diff.LengthSquared();
                diff.Normalize();

                smallObject.Acceleration += diff * massiveObject.Mass / diff.LengthSquared();

            }

        }

        private void screenConstraint(Ship ship)
        {

            Vector2 position = ship.Position;

            if (position.X < minX)
            {

                position.X = maxX + position.X % viewport.Width;

            }

            if (position.X > maxX)
            {

                position.X = minX - position.X % viewport.Width;

            }

            if (position.Y < minY)
            {

                position.Y = maxY + position.Y % viewport.Height;

            }

            if (position.Y > maxY)
            {

                position.Y = minY - position.Y % viewport.Height;

            }

            if (position != ship.Position)
            {

                ship.Position = position;
            }

        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            basicEffect.CurrentTechnique.Passes[0].Apply();

            infoBar.Reset();


            gameObjects.ForEach(gameObject => gameObject.Draw());

            gameObjects.ForEach<IGameObject, Ship>(infoBar.DrawShipInfo);            

            base.Draw(gameTime);
        }
    }


}
