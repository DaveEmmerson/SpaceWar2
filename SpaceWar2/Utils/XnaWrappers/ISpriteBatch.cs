using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public interface ISpriteBatch
    {
        void Begin();
        void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color);
        void End();
    }
}