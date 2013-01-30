using DEMW.SpaceWar2.Core.Controls;
using Microsoft.Xna.Framework.Input;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class ActionHandlerTests
    {
        private IKeyboardHandler _keyboardHandler;
        private ActionHandler _actionHandler;

        [SetUp]
        public void SetUp()
        {
            _keyboardHandler = Substitute.For<IKeyboardHandler>();
            _actionHandler = new ActionHandler(_keyboardHandler);
        }

        [Test]
        public void RegisterContinuousAction_registers_a_key_action_binding()
        {
            var actionTriggeredCount = 0;

            _actionHandler.RegisterContinuousAction(Keys.W, () => actionTriggeredCount++);

            Assert.AreEqual(0, actionTriggeredCount);
            
            _actionHandler.ProcessActions();
            Assert.AreEqual(0, actionTriggeredCount);

            _keyboardHandler.IsPressed(Keys.W).Returns(true);
            _actionHandler.ProcessActions();
            Assert.AreEqual(1, actionTriggeredCount);

            _actionHandler.ProcessActions();
            Assert.AreEqual(2, actionTriggeredCount);
        }

        [Test]
        public void RegisterTriggerAction_registers_a_key_action_binding()
        {
            var actionTriggeredCount = 0;

            _actionHandler.RegisterTriggerAction(Keys.X, () => actionTriggeredCount++);

            Assert.AreEqual(0, actionTriggeredCount);

            _actionHandler.ProcessActions();
            Assert.AreEqual(0, actionTriggeredCount);

            _keyboardHandler.IsNewlyPressed(Keys.X).Returns(true);
            _actionHandler.ProcessActions();
            Assert.AreEqual(1, actionTriggeredCount);

            _keyboardHandler.IsNewlyPressed(Keys.X).Returns(false);
            _actionHandler.ProcessActions();
            Assert.AreEqual(1, actionTriggeredCount);
        }
    }
}
