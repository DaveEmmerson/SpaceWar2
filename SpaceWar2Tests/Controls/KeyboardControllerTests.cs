using DEMW.SpaceWar2.Controls;
using Microsoft.Xna.Framework.Input;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class KeyboardControllerTests
    {
        private IKeyboardHandler _keyboardHandler;
        private KeyboardController _keyboardController;

        [SetUp]
        public void SetUp()
        {
            _keyboardHandler = Substitute.For<IKeyboardHandler>();
            _keyboardHandler.IsPressed(Arg.Any<Keys>()).Returns(false);
            _keyboardController = new KeyboardController(_keyboardHandler);
            _keyboardController.SetMapping(Keys.A, ShipAction.Thrust);
            _keyboardController.SetMapping(Keys.B, ShipAction.FireProjectile);
            _keyboardController.SetMapping(Keys.C, ShipAction.Thrust | ShipAction.FireProjectile);
        }

        [Test]
        public void GetAction_returns_correct_action_when_a_single_key_is_pressed()
        {
            _keyboardHandler.IsPressed(Keys.B).Returns(true);
            
            var action = _keyboardController.GetAction();

            Assert.AreEqual(ShipAction.FireProjectile, action, "Expected action (from 'B' key) was not returned");
        }

        [Test]
        public void GetAction_returns_correct_actions_when_multiple_keys_are_pressed()
        {
            _keyboardHandler.IsPressed(Keys.A).Returns(true);
            _keyboardHandler.IsPressed(Keys.B).Returns(true);

            var action = _keyboardController.GetAction();

            Assert.AreEqual(ShipAction.Thrust | ShipAction.FireProjectile, action, "Expected actions (from 'A' and 'B' keys) were not returned");
        }

        [Test]
        public void GetAction_can_handle_multiple_actions_on_a_single_key()
        {
            _keyboardHandler.IsPressed(Keys.C).Returns(true);

            var action = _keyboardController.GetAction();

            Assert.AreEqual(ShipAction.Thrust | ShipAction.FireProjectile, action, "Expected actions (from 'C' key) were not returned");
        }

        [Test]
        public void GetAction_Returns_None_When_No_Mapped_Keys_Pressed()
        {
            var action = _keyboardController.GetAction();

            Assert.AreEqual(ShipAction.None, action, "Unexpected key press was returned");
        }
    }
}
