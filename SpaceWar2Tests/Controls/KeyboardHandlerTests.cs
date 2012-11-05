using NUnit.Framework;
using DEMW.SpaceWar2.Controls;
using NSubstitute;
using Microsoft.Xna.Framework.Input;

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

            _keyboardHandler = new KeyboardHandler() { Keyboard = _keyboard };
        }

        [TestCase(Keys.A)]
        [TestCase(Keys.Up)]
        [TestCase(Keys.Space)]
        public void IsPressed_returns_correct_value(Keys key)
        {
            Assert.That(!_keyboardHandler.IsPressed(key));
            _keyboard.GetState().Returns(new KeyboardState(key));
            _keyboardHandler.UpdateKeyboardState();
            Assert.That(_keyboardHandler.IsPressed(key));
        }

        [TestCase(Keys.B)]
        [TestCase(Keys.Down)]
        [TestCase(Keys.Enter)]
        public void IsNewlyPressed_returns_correct_value(Keys key)
        {
            Assert.That(!_keyboardHandler.IsPressed(key));
            _keyboard.GetState().Returns(new KeyboardState(key));
            _keyboardHandler.UpdateKeyboardState();
            Assert.That(_keyboardHandler.IsNewlyPressed(key));
            _keyboardHandler.UpdateKeyboardState();
            Assert.That(!_keyboardHandler.IsNewlyPressed(key));
        }
    }
}
