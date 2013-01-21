﻿using DEMW.SpaceWar2.Core;
using DEMW.SpaceWar2.Core.Controls;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests
{
    [TestFixture]
    class GameEngineTests
    {
        private GameEngine _gameEngine;
        
        private IKeyboardHandler _keyboardHandler;
        private IActionHandler _actionHandler;
        
        [SetUp]
        public void SetUp()
        {
            _keyboardHandler = Substitute.For<IKeyboardHandler>();
            _actionHandler = Substitute.For<IActionHandler>();

            _gameEngine = new GameEngine(_keyboardHandler, _actionHandler);
        }

        [Test]
        public void ExecuteGameLoop_updates_keyboard_state()
        {
            _gameEngine.ExecuteGameLoop(new GameTime());

            _keyboardHandler.Received(1).UpdateKeyboardState();
            _actionHandler.Received(1).ProcessActions();
        }
    }
}
