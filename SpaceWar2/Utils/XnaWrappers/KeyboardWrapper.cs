using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public class KeyboardWrapper : IKeyboard
    {
        public KeyboardState GetState()
        {
            return Keyboard.GetState();
        }
    }
}
