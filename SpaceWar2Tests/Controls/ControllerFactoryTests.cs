using DEMW.SpaceWar2.Controls;
using Microsoft.Xna.Framework.Input;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class ControllerFactoryTests
    {
        private IKeyboardHandler _keyboardHandler;
        private ControllerFactory _controllerFactory;
        private IShipController _controller1;
        private IShipController _controller2;

        [SetUp]
        public void SetUp()
        {
            _keyboardHandler = Substitute.For<IKeyboardHandler>();
            _controllerFactory = new ControllerFactory(_keyboardHandler);
            _controller1 = _controllerFactory.Controller1;
            _controller2 = _controllerFactory.Controller2;
        }

        [TestCase(Keys.Up, ShipAction.Thrust, ShipAction.None)]
        [TestCase(Keys.Left, ShipAction.TurnLeft, ShipAction.None)]
        [TestCase(Keys.Right, ShipAction.TurnRight, ShipAction.None)]
        [TestCase(Keys.Down, ShipAction.ReverseThrust, ShipAction.None)]
        [TestCase(Keys.RightControl, ShipAction.FireProjectile, ShipAction.None)]
        [TestCase(Keys.W, ShipAction.None, ShipAction.Thrust)]
        [TestCase(Keys.A, ShipAction.None, ShipAction.TurnLeft)]
        [TestCase(Keys.D, ShipAction.None, ShipAction.TurnRight)]
        [TestCase(Keys.S, ShipAction.None, ShipAction.ReverseThrust)]
        [TestCase(Keys.R, ShipAction.None, ShipAction.FireProjectile)]
        public void Both_default_controller_properties_return_an_appropriate_controller(Keys key, ShipAction controller1ExpectedAction, ShipAction controller2ExpectedAction)
        {
            _keyboardHandler.IsPressed(Arg.Is(key)).Returns(true);

            var controller1Action = _controller1.Action;
            Assert.AreEqual(controller1ExpectedAction, controller1Action);

            var controller2Action = _controller2.Action;
            Assert.AreEqual(controller2ExpectedAction, controller2Action);
        }
    }
}
