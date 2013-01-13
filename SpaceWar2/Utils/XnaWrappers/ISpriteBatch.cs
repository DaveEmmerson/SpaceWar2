using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public interface ISpriteBatch
    {
        void BeginBatch();
        void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color);
        void EndBatch();
    }
}