using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Controls
{
    public class ControllerFactory
    {
        private readonly IKeyboardHandler _keyboardHandler;
        
        public ControllerFactory(IKeyboardHandler keyboardHandler)
        {
            _keyboardHandler = keyboardHandler;
            CreateController1();
            CreateController2();
        }

        private void CreateController1()
        {
            Controller1 = new KeyboardController(_keyboardHandler);

            Controller1.SetMapping(Keys.Up, ShipActions.Thrust);
            Controller1.SetMapping(Keys.Left, ShipActions.TurnLeft);
            Controller1.SetMapping(Keys.Right, ShipActions.TurnRight);
            Controller1.SetMapping(Keys.Down, ShipActions.ReverseThrust);
            Controller1.SetMapping(Keys.RightControl, ShipActions.FireProjectile);
        }

        private void CreateController2()
        {
            Controller2 = new KeyboardController(_keyboardHandler);

            Controller2.SetMapping(Keys.W, ShipActions.Thrust);
            Controller2.SetMapping(Keys.A, ShipActions.TurnLeft);
            Controller2.SetMapping(Keys.D, ShipActions.TurnRight);
            Controller2.SetMapping(Keys.S, ShipActions.ReverseThrust);
            Controller2.SetMapping(Keys.R, ShipActions.FireProjectile);
        }

        public KeyboardController Controller1 { get; private set; }
        public KeyboardController Controller2 { get; private set; }
    }
}
