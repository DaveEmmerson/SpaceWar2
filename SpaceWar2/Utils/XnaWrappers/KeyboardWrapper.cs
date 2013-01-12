using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public class KeyboardWrapper : IKeyboard
    {
        public KeyboardState State
        {
            get { return Keyboard.GetState(); }
        }
    }
}
