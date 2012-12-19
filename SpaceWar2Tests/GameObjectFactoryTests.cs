using System;
using System.Linq;
using DEMW.SpaceWar2;
using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NSubstitute;
using NUnit.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2Tests
{
    [TestFixture]
    class GameObjectFactoryTests
    {
        private GameObjectFactory _gameObjectFactory;
        private ContentManager _contentManager;
        private GraphicsDeviceManager _graphicsDeviceManager;
        private GravitySimulator _gravitySimulator;
        private DrawingManager _drawingManager;
        private Universe _universe;

        [SetUp]
        public void SetUp()
        {
            _contentManager = Substitute.For<ContentManager>();
            _graphicsDeviceManager = Substitute.For<GraphicsDeviceManager>();
            _gravitySimulator = Substitute.For<GravitySimulator>();
            _drawingManager = Substitute.For<DrawingManager>();
            _universe = Substitute.For<Universe>();
            _gameObjectFactory = new GameObjectFactory(_contentManager, _graphicsDeviceManager, _gravitySimulator, _drawingManager, _universe);
        }

        [Test]
        public void Constructor_throws_null_reference_exception_if_any_argument_is_null()
        {
            Assert.Throws<NullReferenceException>(()=> _gameObjectFactory = new GameObjectFactory(null, null, null, null, null));
            Assert.Throws<NullReferenceException>(() => _gameObjectFactory = new GameObjectFactory(null, _graphicsDeviceManager, _gravitySimulator, _drawingManager, _universe));
            Assert.Throws<NullReferenceException>(() => _gameObjectFactory = new GameObjectFactory(_contentManager, null, _gravitySimulator, _drawingManager, _universe));
            Assert.Throws<NullReferenceException>(() => _gameObjectFactory = new GameObjectFactory(_contentManager, _graphicsDeviceManager, null, _drawingManager, _universe));
            Assert.Throws<NullReferenceException>(() => _gameObjectFactory = new GameObjectFactory(_contentManager, _graphicsDeviceManager, _gravitySimulator, null, _universe));
            Assert.Throws<NullReferenceException>(() => _gameObjectFactory = new GameObjectFactory(_contentManager, _graphicsDeviceManager, _gravitySimulator, _drawingManager, null));
        }

        [Test]
        public void GameObjects_is_initially_empty()
        {
            Assert.IsEmpty(_gameObjectFactory.GameObjects);   
        }

        [Test]
        public void CreateSun_creates_a_Sun_with_the_specified_properties_and_adds_it_to_GameObjects()
        {
            var position = new Vector2(100, 200);
            var color = Color.Orange;
            var mass = 20;
         
            var sun = _gameObjectFactory.CreateSun(position, Color.Orange, mass);

            Assert.AreEqual(position, sun.Position);
            Assert.AreEqual(color, sun.Color);
            Assert.AreEqual(mass, sun.Mass);
            Assert.AreEqual(25, sun.Radius);

            _contentManager.Received(1).Load<Model>("Models/Sun");
            _gravitySimulator.Received(1).RegisterSource(sun);
            _drawingManager.Received(1).Register(sun);
            
            Assert.IsNotEmpty(_gameObjectFactory.GameObjects);
            Assert.AreEqual(1, _gameObjectFactory.GameObjects.Count);
            Assert.Contains(sun, _gameObjectFactory.GameObjects.ToList());
        }
    }
}
