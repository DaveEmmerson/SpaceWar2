﻿using DEMW.SpaceWar2.Core;
using DEMW.SpaceWar2.Core.Controls;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.Core.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using NUnit.Framework;
using System.Linq;

namespace DEMW.SpaceWar2Tests
{
    [TestFixture]
    class GameObjectFactoryTests
    {
        private IGameObjectFactory _gameObjectFactory;
        private IContentManager _contentManager;
        private IGraphicsFactory _graphicsFactory;
        private IGravitySimulator _gravitySimulator;
        private IDrawingManager _drawingManager;
        private IUniverse _universe;
        private IShipComponentFactory _shipComponentFactory;

        [SetUp]
        public void SetUp()
        {
            _contentManager = Substitute.For<IContentManager>();
            _graphicsFactory = Substitute.For<IGraphicsFactory>();
            _gravitySimulator = Substitute.For<IGravitySimulator>();
            _drawingManager = Substitute.For<IDrawingManager>();
            _universe = Substitute.For<IUniverse>();
            _shipComponentFactory = Substitute.For<IShipComponentFactory>();

            _gameObjectFactory = new GameObjectFactory(_contentManager, _graphicsFactory, _gravitySimulator, _drawingManager, _universe, _shipComponentFactory);
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
            const int mass = 20;
         
            var sun = _gameObjectFactory.CreateSun(position, color, mass);

            Assert.AreEqual(position, sun.Position);
            Assert.AreEqual(color, sun.Color);
            Assert.AreEqual(mass, sun.Mass);
            Assert.AreEqual(25, sun.Radius);

            Assert.IsNotEmpty(_gameObjectFactory.GameObjects);
            Assert.AreEqual(1, _gameObjectFactory.GameObjects.Count);
            Assert.Contains(sun, _gameObjectFactory.GameObjects.ToList());
        }

        [Test]
        public void CreateSun_registers_newly_created_sun_with_managers_and_simulators()
        {
            var sun = _gameObjectFactory.CreateSun(Vector2.Zero, Color.Red, 25);

            _contentManager.Received(1).Load<Model>("Models/Sun");
            _gravitySimulator.Received(1).RegisterSource(sun);
            _drawingManager.Received(1).Register(sun);
        }

        [Test]
        public void CreateShip_creates_a_Ship_with_the_specified_properties_and_adds_it_to_GameObjects()
        {
            const string name = "Ship 1";
            var position = new Vector2(100, 200);
            var velocity = new Vector2(300, 400);
            var color = Color.Blue;
            var controller = Substitute.For<IShipController>();

            var ship = (Ship) _gameObjectFactory.CreateShip(name, position, velocity, color, controller);

            Assert.AreEqual(name, ship.Name);
            Assert.AreEqual(position, ship.Position);
            Assert.AreEqual(velocity, ship.Velocity);
            Assert.AreEqual(color, ship.Color);
            Assert.AreEqual(16, ship.Radius);

            Assert.IsNotEmpty(_gameObjectFactory.GameObjects);
            Assert.AreEqual(1, _gameObjectFactory.GameObjects.Count);
            Assert.Contains(ship, _gameObjectFactory.GameObjects.ToList());
        }

        [Test]
        public void CreateShip_registers_newly_created_ship_with_managers_and_simulators()
        {
            var ship = _gameObjectFactory.CreateShip("Ship 1", Vector2.Zero, Vector2.Zero, Color.Red, null);

            _contentManager.Received(1).Load<Model>("Models/Saucer");
            _gravitySimulator.Received(1).RegisterParticipant(ship);
            _universe.Received(1).Register(ship);
            _drawingManager.Received(1).Register(ship);
        }

        [Test]
        public void DestroyAll_removes_all_objects_from_GameObjects()
        {
            var ship = _gameObjectFactory.CreateShip("Ship 1", Vector2.Zero, Vector2.Zero, Color.Red, null);
            var sun = _gameObjectFactory.CreateSun(Vector2.Zero, Color.Red, 25);

            _gameObjectFactory.DestroyAll(x => true);

            Assert.IsEmpty(_gameObjectFactory.GameObjects);

            _gravitySimulator.Received(1).Unregister(ship);
            _universe.Received(1).Unregister(ship);
            _drawingManager.Received(1).Unregister(ship);

            _gravitySimulator.Received(1).Unregister(sun);
            _universe.Received(1).Unregister(sun);
            _drawingManager.Received(1).Unregister(sun);
        }

        [Test]
        public void DestroyAll_removes_only_the_objects_that_match_the_provided_predicate()
        {
            var ship = _gameObjectFactory.CreateShip("Ship 1", Vector2.Zero, Vector2.Zero, Color.Red, null);
            var sun = _gameObjectFactory.CreateSun(Vector2.Zero, Color.Red, 25);
            
            _gameObjectFactory.DestroyAll(x => x is Ship);

            Assert.AreEqual(1, _gameObjectFactory.GameObjects.Count);

            _gravitySimulator.Received(1).Unregister(ship);
            _universe.Received(1).Unregister(ship);
            _drawingManager.Received(1).Unregister(ship);

            _gravitySimulator.DidNotReceive().Unregister(sun);
            _universe.DidNotReceive().Unregister(sun);
            _drawingManager.DidNotReceive().Unregister(sun);
        }
    }
}
