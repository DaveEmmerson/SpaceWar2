using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Controls
{
    public interface IKeyboardHandler
    {
        void UpdateKeyboardState();
        bool IsPressed(Keys key);
        bool IsNewlyPressed(Keys key);
    }
}
