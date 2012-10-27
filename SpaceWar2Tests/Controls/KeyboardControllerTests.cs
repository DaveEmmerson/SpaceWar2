using NUnit.Framework;
using NSubstitute;
using DEMW.SpaceWar2.Controls;
using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2Tests.Controls
{
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
        public void GetAction_Returns_Correct_Action_Single_Action()
        {
            _keyboardHandler.IsPressed(Keys.B).Returns(true);
            
            var action = _keyboardController.GetAction();

            Assert.AreEqual(ShipAction.FireProjectile, action, "B key pressed");
        }

        [Test]
        public void GetAction_Returns_Correct_Action_Multiple_Action()
        {
            _keyboardHandler.IsPressed(Keys.C).Returns(true);

            var action = _keyboardController.GetAction();

            Assert.AreEqual(ShipAction.Thrust | ShipAction.FireProjectile, action, "C key pressed");
        }
    }
}
