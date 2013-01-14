using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Core.Utils.XnaWrappers
{
    internal class KeyboardWrapper : IKeyboard
    {
        public KeyboardState State
        {
            get { return Keyboard.GetState(); }
        }
    }
}
