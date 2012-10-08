using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Controls
{
    internal class ControllerFactory
    {
        private readonly KeyboardHandler _keyboardHandler;
        
        internal ControllerFactory(KeyboardHandler keyboardHandler)
        {
            _keyboardHandler = keyboardHandler;
            CreateController1();
            CreateController2();
        }

        private void CreateController1()
        {
            Controller1 = new KeyboardController(_keyboardHandler);

            Controller1.SetMapping(Keys.Up, ShipAction.Thrust);
            Controller1.SetMapping(Keys.Left, ShipAction.TurnLeft);
            Controller1.SetMapping(Keys.Right, ShipAction.TurnRight);
            Controller1.SetMapping(Keys.Down, ShipAction.ReverseThrust);
            Controller1.SetMapping(Keys.RightControl, ShipAction.FireProjectile);
        }

        private void CreateController2()
        {
            Controller2 = new KeyboardController(_keyboardHandler);

            Controller2.SetMapping(Keys.W, ShipAction.Thrust);
            Controller2.SetMapping(Keys.A, ShipAction.TurnLeft);
            Controller2.SetMapping(Keys.D, ShipAction.TurnRight);
            Controller2.SetMapping(Keys.S, ShipAction.ReverseThrust);
            Controller2.SetMapping(Keys.R, ShipAction.FireProjectile);
        }

        internal KeyboardController Controller1 { get; private set; }
        internal KeyboardController Controller2 { get; private set; }
    }
}
