using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Controls
{
    public class KeyboardWrapper : IKeyboard
    {
        public KeyboardState GetState()
        {
            return Keyboard.GetState();
        }
    }
}
