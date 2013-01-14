using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Core.Controls
{
    public interface IKeyboardHandler
    {
        void UpdateKeyboardState();
        bool IsPressed(Keys key);
        bool IsNewlyPressed(Keys key);
    }
}
