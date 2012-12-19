﻿using DEMW.SpaceWar2.Controls;
using Microsoft.Xna.Framework.Input;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class KeyboardHandlerTests
    {
        private IKeyboard _keyboard;
        private KeyboardHandler _keyboardHandler;

        [SetUp]
        public void SetUp()
        {
            _keyboard = Substitute.For<IKeyboard>();

            _keyboard.GetState().Returns(new KeyboardState());

            _keyboardHandler = new KeyboardHandler(_keyboard);
        }

        [TestCase(Keys.A)]
        [TestCase(Keys.Up)]
        [TestCase(Keys.Space)]
        public void IsPressed_returns_correct_value_even_if_repeatedly_called(Keys key)
        {
            _keyboard.GetState().Returns(new KeyboardState(key));
            _keyboardHandler.UpdateKeyboardState();
            _keyboard.Received(2).GetState(); // constructor plus one update keyboard state
            Assert.IsTrue(_keyboardHandler.IsPressed(key));
            Assert.IsTrue(_keyboardHandler.IsPressed(key));
        }

        [TestCase(Keys.B)]
        [TestCase(Keys.Down)]
        [TestCase(Keys.Enter)]
        public void IsNewlyPressed_returns_correct_value_even_if_repeatedly_called(Keys key)
        {
            _keyboard.GetState().Returns(new KeyboardState(key));
            _keyboardHandler.UpdateKeyboardState();
            Assert.IsTrue(_keyboardHandler.IsNewlyPressed(key));
            Assert.IsTrue(_keyboardHandler.IsNewlyPressed(key));
            _keyboardHandler.UpdateKeyboardState();
            _keyboard.Received(3).GetState(); // constructor plus two update keyboard states 
            Assert.IsFalse(_keyboardHandler.IsNewlyPressed(key)); // the key is still pressed; the handler is still returning the key. It is not newly presssed though, as there has been an update.
        }
    }
}
