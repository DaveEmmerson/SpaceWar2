using System;
using DEMW.SpaceWar2.Core;
using DEMW.SpaceWar2.Core.Controls;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests
{
    [TestFixture]
    class GameEngineTests
    {
        private GameEngine _gameEngine;

        private IUniverse _universe;
        private IGravitySimulator _gravitySimulator;
        private IGameObjectFactory _gameObjectFactory;
        private IKeyboardHandler _keyboardHandler;
        private IActionHandler _actionHandler;
        
        [SetUp]
        public void SetUp()
        {
            _universe = Substitute.For<IUniverse>();
            _gravitySimulator = Substitute.For<IGravitySimulator>();
            _gameObjectFactory = Substitute.For<IGameObjectFactory>();
            _keyboardHandler = Substitute.For<IKeyboardHandler>();
            _actionHandler = Substitute.For<IActionHandler>();

            _gameEngine = new GameEngine(_universe, _gravitySimulator, _gameObjectFactory, _keyboardHandler, _actionHandler);
        }

        [Test]
        public void ExecuteGameLoop_calls_all_the_components_to_run_the_game()
        {
            _gameEngine.ExecuteGameLoop(new GameTime());

            _keyboardHandler.Received(1).UpdateKeyboardState();
            _actionHandler.Received(1).ProcessActions();
            _gameObjectFactory.Received(1).DestroyAll(Arg.Any<Predicate<IGameObject>>());
            _gravitySimulator.Received(1).Simulate();
            _universe.Received(1).Update();
            var temp = _gameObjectFactory.Received(1).GameObjects;
            Assert.IsNull(temp);
        }

        [Test]
        public void ExecuteGameLoop_when_paused_only_calls_certain_components()
        {
            _keyboardHandler.IsNewlyPressed(Keys.Space).Returns(true);

            var actionHandler = new ActionHandler(_keyboardHandler);
            _gameEngine = new GameEngine(_universe, _gravitySimulator, _gameObjectFactory, _keyboardHandler, actionHandler);

            _gameEngine.ExecuteGameLoop(new GameTime());

            _keyboardHandler.Received(1).UpdateKeyboardState();
            
            _gameObjectFactory.DidNotReceive().DestroyAll(Arg.Any<Predicate<IGameObject>>());
            _gravitySimulator.DidNotReceive().Simulate();
            _universe.DidNotReceive().Update();
            var temp = _gameObjectFactory.DidNotReceive().GameObjects;
            Assert.IsNull(temp);
        }

        [Test]
        public void ResetGame_destroys_gameObjects_and_creates_new_ones()
        {
            _gameEngine.ResetGame();

            //Don't really care about what it calls for now as that will probably be data driven in the future
            //TODO replace this test with a more specific one when setting up the game world uses some sort of world definition
            _gameObjectFactory.Received(1).DestroyAll(Arg.Any<Predicate<IGameObject>>());
            _gameObjectFactory.ReceivedWithAnyArgs().CreateShip("", Vector2.Zero, Vector2.Zero, Color.Transparent, null);
            _gameObjectFactory.ReceivedWithAnyArgs().CreateSun(Vector2.Zero, Color.Transparent, 0f);
        }
    }
}
