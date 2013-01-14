using DEMW.SpaceWar2.Controls;
using DEMW.SpaceWar2.Core.Controls;
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

        [TestCase(Keys.Up, ShipActions.Thrust, ShipActions.None)]
        [TestCase(Keys.Left, ShipActions.TurnLeft, ShipActions.None)]
        [TestCase(Keys.Right, ShipActions.TurnRight, ShipActions.None)]
        [TestCase(Keys.Down, ShipActions.ReverseThrust, ShipActions.None)]
        [TestCase(Keys.RightControl, ShipActions.FireProjectile, ShipActions.None)]
        [TestCase(Keys.W, ShipActions.None, ShipActions.Thrust)]
        [TestCase(Keys.A, ShipActions.None, ShipActions.TurnLeft)]
        [TestCase(Keys.D, ShipActions.None, ShipActions.TurnRight)]
        [TestCase(Keys.S, ShipActions.None, ShipActions.ReverseThrust)]
        [TestCase(Keys.R, ShipActions.None, ShipActions.FireProjectile)]
        public void Both_default_controller_properties_return_an_appropriate_controller(Keys key, ShipActions controller1ExpectedActions, ShipActions controller2ExpectedActions)
        {
            _keyboardHandler.IsPressed(Arg.Is(key)).Returns(true);

            var controller1Actions = _controller1.Actions;
            Assert.AreEqual(controller1ExpectedActions, controller1Actions);

            var controller2Actions = _controller2.Actions;
            Assert.AreEqual(controller2ExpectedActions, controller2Actions);
        }
    }
}
